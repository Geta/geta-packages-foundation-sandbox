using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Foundation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Main<Startup>(args);
        }
        
        
        public static void Main<TStartup>(string[] args) where TStartup: class
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            if (isDevelopment)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Warning()
                    .WriteTo.File("App_Data/log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            }


            CreateHostBuilder<TStartup>(args, isDevelopment).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args, bool isDevelopment) where TStartup: class
        {
            if (isDevelopment)
            {
                return Host.CreateDefaultBuilder(args)
                    .ConfigureCmsDefaults()
                    .UseSerilog()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<TStartup>();
                    });
            }
            else
            {
                return Host.CreateDefaultBuilder(args)
                    .ConfigureCmsDefaults()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<TStartup>();
                    });
            }
        }
    }
}
