using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using FederationGatewayApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
            var serverName = combinedEnvironmentName.LastOrDefault();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.{serverName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddControllersAsServices();
            // configure jwt authentication
            services.AddAuthorization();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(DeprecatedCheckAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // Add API versioning to as service to your project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
                config.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader()
                        {
                            HeaderNames = { "api-version" }
                        },
                        new QueryStringApiVersionReader("v")
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API version 1", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "API version 2", Version = "v2" });
                c.OperationFilter<RemoveVersionParameterAttribute>();
                c.DocumentFilter<FilterRoutesDocumentFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathAttribute>();
                c.EnableAnnotations();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(typeof(CountryMapper));
            services.AddSingleton(new AuthOptions(_env.IsDevelopment()));
            services.AddSingleton<IAppSettingsConfig, AppSettingsConfig>();
            services.AddSingleton(Configuration);
            services.AddScoped<MobileAuthorizationAttribute>();
            services.AddScoped<DeprecatedCheckAttribute>();
            services.AddScoped<AuthorizationAttribute>();

            services.AddHsts(options =>
            {
                options.MaxAge = DateTime.Now.AddYears(1) - DateTime.Now;
                options.IncludeSubDomains = true;
            });

            var connectionString = Configuration["SQLConnectionString"];
            services.AddScoped<ICacheOperations, CacheOperations>();
            services.AddScoped<IDatabaseKeysToBinaryStreamMapperService, DatabaseKeysToBinaryStreamMapperService>();
            services.AddScoped(typeof(ITemporaryExposureKeyRepository), typeof(TemporaryExposureKeyRepository));
            services.AddScoped<IExposureKeyMapper, ExposureKeyMapper>();
            services.AddScoped<IExposureKeyValidator, ExposureKeyValidator>();
            services.AddScoped<ILogMessageValidator, LogMessageValidator>();
            services.AddScoped<IAppleService, AppleService>();
            services.AddScoped<IKeysListToMemoryStreamConverter, KeysListToMemoryStreamConverter>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPackageBuilderService, PackageBuilderService>();
            services.AddScoped<IAddTemporaryExposureKeyService, AddTemporaryExposureKeyService>();
            services.AddScoped<IZipFileInfoService, ZipFileInfoService>();
            services.AddScoped<IFileSystem, FileSystem>();

            services.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseSqlServer(connectionString, x => x.MigrationsAssembly("DIGNDB.App.SmitteStop.DAL")));
            services.AddScoped<DigNDB_SmittestopContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IDataAccessLoggingService<>), typeof(DataAccessLoggingService<>));
            services.AddScoped<ICountryRepository, CountryRepository>();


            services.AddSingleton<IExposureConfigurationService, ExposureConfigurationService>();
            services.AddSingleton<IExportKeyConfigurationService, ExportKeyConfigurationService>();
            services.AddSingleton<IKeyValidationConfigurationService, KeyValidationConfigurationService>();

            services.AddScoped<IKeyValidator, KeyValidator>();
            services.AddScoped<IEpochConverter, EpochConverter>();
            services.AddScoped<IRiskCalculator, RiskCalculator>();

            services.AddMemoryCache();

            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .ToList();
            InjectionChecker.CheckIfAreAnyDependenciesAreMissing(services, controllers);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddFile(@"" + Configuration.GetSection(AppSettingsConfig.AppSettingsSectionName)
                .GetValue<string>("logsApiPath"));

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
                c.SwaggerEndpoint("v1/swagger.json", "API Version 1");
                c.SwaggerEndpoint("v2/swagger.json", "API Version 2");
            });

            app.UseAuthorization();
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var exposureConfigurationService = app.ApplicationServices.GetService<IExposureConfigurationService>();
            exposureConfigurationService.SetConfiguration(Configuration);

            var exportKeyConfigurationService = app.ApplicationServices.GetService<IExportKeyConfigurationService>();
            exportKeyConfigurationService.SetConfiguration(Configuration);
            var keyValidationConfigurationService = app.ApplicationServices.GetService<IKeyValidationConfigurationService>();
            keyValidationConfigurationService.SetConfiguration(Configuration);
        }
    }
}
