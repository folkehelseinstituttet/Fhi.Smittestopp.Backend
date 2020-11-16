using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using FederationGatewayApi.Services.Settings;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Linq;
using System.Reflection;
using DIGNDB.App.SmitteStop.DAL.DependencyInjection;
using FederationGatewayApi;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public class HangfireStartup
    {
        private HangfireConfig _hangfireConfig { get; set; }

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
            services.AddGatewayDependencies();

            InjectionChecker.CheckIfAreAnyDependenciesAreMissing(services, Assembly.GetExecutingAssembly(), typeof(ControllerBase));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(_hangfireConfig.LogsPath);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseHangfireServer(options: new BackgroundJobServerOptions { WorkerCount = 1 });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ScheduledJobsConfiguration.ConfigureScheduledJobs(_hangfireConfig);
        }
    }
}
