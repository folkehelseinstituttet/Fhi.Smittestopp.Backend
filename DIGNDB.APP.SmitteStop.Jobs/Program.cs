using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logBuilder) =>
                {
                    logBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logBuilder.ClearProviders();
                    logBuilder.AddConsole();
                    logBuilder.AddTraceSource("Information, ActivityTracing");
                    logBuilder.AddEventLog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<HangfireStartup>();
                }).ConfigureWebHost(config =>
                {
                    config.UseUrls("http://localhost:5050");
                }).UseWindowsService();
    }
}
