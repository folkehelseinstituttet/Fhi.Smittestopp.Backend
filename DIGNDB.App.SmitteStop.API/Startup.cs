using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.DependencyInjection;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            var combinedEnvironmentName = env.EnvironmentName.Split('.');

            var environmentName = combinedEnvironmentName.FirstOrDefault();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddConfiguration(configuration);
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppSettingsConfig appSettingsConfig, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var loggingConfig = Configuration.GetSection(nameof(Logging)).Get<Logging>();
            Enum.TryParse(loggingConfig.LogLevel.Default, out LogLevel logLevel);
            loggerFactory.AddFile(appSettingsConfig.LogsApiPath, logLevel,
                fileSizeLimitBytes: appSettingsConfig.LogFileSizeLimitBytes);

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

            // Who are you...
            app.UseAuthentication();
            // ...what you may access
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Health checks mapped to different endpoints
                MapHealthChecks(endpoints);
            });
        }

        #region Health check mapping and response write

        /// <summary>
        /// Name of policy used for health check authorization
        /// </summary>
        public const string HealthCheckAccessPolicyName = "HealtCheckAccess";

        public const string DatabaseTag = "database";
        public const string DatabasePattern = "/health/database";
        public const string HangFireTag = "hangfire";
        public const string HangFirePattern = "/health/hangfire";
        public const string LogFilesTag = "logfiles";
        /// <summary>
        /// Path to health check endpoint for checking log files
        /// </summary>
        public const string LogFilesPattern = "/health/logfiles";

        public const string ZipFilesTag = "zipfiles";
        /// <summary>
        /// Path to health check endpoint for checking zip files
        /// </summary>
        public const string ZipFilesPattern = "/health/zipfiles";

        public const string NumbersTodayTag = "numberstoday";
        /// <summary>
        /// Path to health check endpoint for checking today's numbers
        /// </summary>
        public const string NumbersTodayPattern = "/health/numberstoday";

        public const string RollingStartNumberTag = "rollingstartnumber";
        /// <summary>
        /// Path to health check endpoint for checking rollingStartNumber
        /// </summary>
        public const string RollingStartNumberPattern = "/health/rollingstartnumber";

        private const string SystemTag = "system";
        private const string SystemPattern = "/health/system";

        private static void MapHealthChecks(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks(DatabasePattern, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(DatabaseTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(HangFirePattern, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(HangFireTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(LogFilesPattern, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(LogFilesTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(ZipFilesPattern, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(ZipFilesTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(NumbersTodayPattern, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(NumbersTodayTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(RollingStartNumberPattern, new HealthCheckOptions()
            {
                Predicate = check => check.Tags.Contains(RollingStartNumberTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);

            endpoints.MapHealthChecks(SystemPattern, new HealthCheckOptions()
            {
                Predicate = check => check.Tags.Contains(SystemTag),
                ResponseWriter = WriteHealthCheckResponse
            }).RequireAuthorization(HealthCheckAccessPolicyName);
        }

        private static Task WriteHealthCheckResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, options))
            {
                writer.WriteStartObject();
                writer.WriteString("status", result.Status.ToString());
                writer.WriteStartObject("results");
                foreach (var entry in result.Entries)
                {
                    // Status, description, and exception (if any)
                    writer.WriteStartObject(entry.Key);
                    writer.WriteString("status", entry.Value.Status.ToString());
                    writer.WriteString("description", entry.Value.Description);

                    // Exception
                    if (entry.Value.Exception != null)
                    {
                        writer.WriteStartObject("healthException");
                        writer.WriteString("message", entry.Value.Exception.Message);
                        writer.WriteString("stackTrace", entry.Value.Exception.StackTrace);
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();

                    // Write data from health checks to response
                    writer.WriteStartObject("data");
                    try
                    {
                        foreach (var (key, value) in entry.Value.Data)
                        {
                            if (value is string)
                            {
                                var val = value.ToString();
                                writer.WriteString(key, val);
                            }
                            else
                            {
                                var itemValue = JsonSerializer.Serialize(value, value?.GetType() ?? typeof(object));
                                writer.WriteString(key, itemValue);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var errorMessage = $"Error in writing to health check response: {e.Message} - {e.StackTrace}";
                        writer.WriteString("Error in writing health check response", errorMessage);
                    }

                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            var json = Encoding.UTF8.GetString(stream.ToArray());

            return context.Response.WriteAsync(json);
        }

        #endregion
    }
}
