using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace DIGNDB.App.SmitteStop.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
                ConfigureLogging((hostingContext, logBuilder) =>
                {
                    logBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logBuilder.ClearProviders();
                    logBuilder.AddConsole();
                    logBuilder.AddTraceSource("Information, ActivityTracing");
                    // because framework is not  using settings from appsettings.json
                    var eventLogSettings = new EventLogSettings()
                    {
                        SourceName = hostingContext.Configuration["Logging:EventLog:SourceName"],
                        LogName = hostingContext.Configuration["Logging:EventLog:LogName"],
                        MachineName = hostingContext.Configuration["Logging:EventLog:MachineName"]
                    };
                    logBuilder.AddEventLog(eventLogSettings);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                });
    }
}
