namespace Foundation.AppHost.Configurations;

public class AppHostConfiguration
{
    public string SqlServerName { get; set; } = null!;
    public int? SqlServerPort { get; set; }
    public string CmsDatabaseName { get; set; } = null!;
    public string CommerceDatabaseName { get; set; } = null!;
    public string WebName { get; set; } = null!;
    public int? WebPort { get; set; }
    public string? WebScheme { get; set; }
}