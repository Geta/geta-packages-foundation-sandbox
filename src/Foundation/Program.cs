using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            CreateHostBuilder<TStartup>(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args, Action<IWebHostBuilder> webHostBuilderConfigure = null) where TStartup: class
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureCmsDefaults()
                .ConfigureLogging(builder =>
                {
                    builder.AddOpenTelemetry(logging =>
                    {
                        logging.IncludeFormattedMessage = true;
                        logging.IncludeScopes = true;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    webHostBuilderConfigure?.Invoke(webBuilder);
                });
        }
    }
}
