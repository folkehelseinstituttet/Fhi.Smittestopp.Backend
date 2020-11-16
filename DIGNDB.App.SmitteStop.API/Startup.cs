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
                config.DefaultApiVersion = new ApiVersion(3, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
                config.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader()
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API version 1", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "API version 2", Version = "v2" });
                c.SwaggerDoc("v3", new OpenApiInfo { Title = "API version 3", Version = "v3" });
                c.OperationFilter<RemoveVersionParameterAttribute>();
                c.DocumentFilter<FilterRoutesDocumentFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathAttribute>();
                c.EnableAnnotations();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            RegisterConfigurationDtos(services);

            var jwtAuthorizationConfig = Configuration.GetSection(nameof(JwtAuthorization)).Get<JwtAuthorization>();
            ModelValidator.ValidateContract(jwtAuthorizationConfig);
            services.AddSingleton(jwtAuthorizationConfig);

            services.AddAutoMapper(typeof(CountryMapper));
            services.AddSingleton(new AuthOptions(_env.IsDevelopment()));
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

            services.AddScoped<IKeyValidator, KeyValidator>();
            services.AddScoped<IEpochConverter, EpochConverter>();
            services.AddScoped<IRiskCalculator, RiskCalculator>();

            services.AddCoreDependencies();
            services.AddDALDependencies();

            services.AddMemoryCache();

            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .ToList();
            InjectionChecker.CheckIfAreAnyDependenciesAreMissing(services, controllers);
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

            loggerFactory.AddFile(appSettingsConfig.LogsApiPath);

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
                c.SwaggerEndpoint("v3/swagger.json", "API Version 3");
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
