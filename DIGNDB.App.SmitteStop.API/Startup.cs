using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.DependencyInjection;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using FederationGatewayApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using DIGNDB.App.SmitteStop.Domain;

namespace DIGNDB.App.SmitteStop.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            var combinedEnvironmentName = env.EnvironmentName.Split('.');

            var environmentName = combinedEnvironmentName.FirstOrDefault();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAPIDependencies(Configuration, _env);
            services.AddApiServiceCollectionDependencies();

            services.AddCoreDependencies();
            services.AddDALDependencies();

            InjectionChecker.CheckIfAreAnyDependenciesAreMissing(services, Assembly.GetExecutingAssembly(), typeof(ControllerBase));
        }

        private void RegisterConfigurationDtos(IServiceCollection services)
        {
            var appsettingsConfig = Configuration.GetSection("AppSettings").Get<AppSettingsConfig>();
            ModelValidator.ValidateContract(appsettingsConfig);
            services.AddSingleton(appsettingsConfig);
            services.AddSingleton<IOriginSpecificSettings>(appsettingsConfig);
            services.AddSingleton<IZipPackageBuilderConfig>(appsettingsConfig);

            var logValidationRulesConfig = Configuration.GetSection("LogValidationRules").Get<LogValidationRulesConfig>();
            ModelValidator.ValidateContract(logValidationRulesConfig);
            services.AddSingleton(logValidationRulesConfig);

            var keyValidationRulesConfig = Configuration.GetSection("KeyValidationRules").Get<KeyValidationConfiguration>();
            ModelValidator.ValidateContract(keyValidationRulesConfig);
            services.AddSingleton(keyValidationRulesConfig);

            services.AddSingleton<IExposureConfigurationService>(new ExposureConfigurationService(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppSettingsConfig appSettingsConfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var loggingConfig = Configuration.GetSection(nameof(Logging)).Get<Logging>();
            Enum.TryParse(loggingConfig.LogLevel.Default, out LogLevel logLevel);
            loggerFactory.AddFile(appSettingsConfig.LogsApiPath, logLevel);

            app.UseHttpsRedirection();

            app.UseHsts();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("swagger/v1/swagger.json", "API Version 1");
                c.SwaggerEndpoint("swagger/v2/swagger.json", "API Version 2");
                c.SwaggerEndpoint("swagger/v3/swagger.json", "API Version 3");
            });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
