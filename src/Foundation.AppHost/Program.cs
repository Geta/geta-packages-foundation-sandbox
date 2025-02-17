using Foundation.AppHost.Configurations;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

const string srcDirectoryPath = "../../../../src";
const string foundationDirectoryPath = "../Foundation";

var projectDirectoryPath = Directory.Exists(srcDirectoryPath)
    ? Directory
        .GetDirectories(srcDirectoryPath, "*.Web")
        .FirstOrDefault() ?? foundationDirectoryPath
    : foundationDirectoryPath;

var config = new ConfigurationBuilder()
    .SetBasePath(Path.GetFullPath(projectDirectoryPath))
    .AddJsonFile("appsettings.json")
    .Build()
    .GetSection("AppHost")
    .Get<AppHostConfiguration>();

if (config == null)
{
    throw new NullReferenceException("Please fill appsettings.json in web project");
}

if (string.IsNullOrEmpty(config.CmsDatabaseName))
{
    throw new NullReferenceException("CmsDatabaseName cannot be null or empty");
}

if (string.IsNullOrEmpty(config.CommerceDatabaseName))
{
    throw new NullReferenceException("CommerceDatabaseName cannot be null or empty");
}

var sqlserver = builder.AddSqlServer(config.SqlServerName, port: config.SqlServerPort)
    // Mount the init scripts directory into the container.
    .WithBindMount("../Foundation/docker/build-script", "/usr/config")
    // Mount the SQL scripts directory into the container so that the init scripts run.
    .WithBindMount("../Foundation/docker/sql-script", "/docker-entrypoint-initdb.d")
    .WithEnvironment("cms_db", config.CmsDatabaseName)
    .WithEnvironment("commerce_db", config.CommerceDatabaseName)
    // Run the custom entrypoint script on startup.
    .WithEntrypoint("/usr/config/entrypoint.sh")
    // Configure the container to store data in a volume so that it persists across instances.
    .WithDataVolume()
    // Keep the container running between app host sessions.
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName(config.SqlServerName);

var cmsDatabase = sqlserver.AddDatabase(config.CmsDatabaseName);
var commerceDatabase = sqlserver.AddDatabase(config.CommerceDatabaseName);

var csprojPath = Directory.GetFiles(projectDirectoryPath, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

if (string.IsNullOrEmpty(csprojPath))
{
    throw new DirectoryNotFoundException(".csproj file not found");
}

var buildConfiguration =
#if DEBUG
    "Debug";
#else
    "Release";
#endif

RunProcess("dotnet", $"build -c {buildConfiguration}", projectDirectoryPath);

var project = builder.AddProject(config.WebName, csprojPath)
    .WithEnvironment("ConnectionStrings__EPiServerDB", cmsDatabase.Resource.ConnectionStringExpression)
    .WithEnvironment("ConnectionStrings__EcfSqlConnection", commerceDatabase.Resource.ConnectionStringExpression)
    .WaitFor(sqlserver);

if (config.WebPort != null)
{
    project.WithEndpoint(port: config.WebPort);
}

builder.Build().Run();
return;

void RunProcess(string filename, string arguments, string path)
{
    Process process = new Process();

    process.StartInfo = new ProcessStartInfo
    {
        FileName = filename,
        Arguments = arguments,
        WorkingDirectory = Path.GetFullPath(path),
        UseShellExecute = true,
        CreateNoWindow = false
    };

    process.Start();
    process.WaitForExit();
}