using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.DAL.DependencyInjection;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public class HangfireStartup
    {
        private IConfiguration _configuration { get; }

        public HangfireStartup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJobsDependencies(_configuration);
            services.AddCoreDependencies();
            services.AddDALDependencies();

            InjectionChecker.CheckIfAreAnyDependenciesAreMissing(services, Assembly.GetExecutingAssembly(), typeof(ControllerBase));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, HangfireConfig hangfireConfig)
        {
            loggerFactory.AddFile(hangfireConfig.LogsPath);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard();

            // app.UseHttpsRedirection(); // TODO test

            app.UseRouting();

            app.UseHangfireServer(options: new BackgroundJobServerOptions { WorkerCount = 1 });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ScheduledJobsConfiguration.ConfigureScheduledJobs(hangfireConfig);
        }
    }
}
