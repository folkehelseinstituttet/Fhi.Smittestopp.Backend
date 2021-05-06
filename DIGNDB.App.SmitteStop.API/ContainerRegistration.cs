using AnonymousTokens.Core.Services;
using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using DIGNDB.App.SmitteStop.API.HealthChecks;
using DIGNDB.App.SmitteStop.API.Mappers;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using FederationGatewayApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DIGNDB.App.SmitteStop.API
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddAPIDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddControllers().AddControllersAsServices();

            services.AddAuthentication(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme).AddNoOperationAuthentication();
            // Configure jwt authentication and adding policy for health check endpoints
            services.AddAuthorization(options =>
            {
                // Adding policy for health check access
                options.AddPolicy(Startup.HealthCheckAccessPolicyName, policy =>
                    policy.AddAuthenticationSchemes(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)
                        .Requirements.Add(new HealthCheckAuthorizationRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, HealthCheckAuthorizationHandler>();


            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(DeprecatedCheckAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
            services.AddHsts(options =>
            {
                options.MaxAge = DateTime.Now.AddYears(1) - DateTime.Now;
                options.IncludeSubDomains = true;
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
            services.AddSingleton(new AuthOptions(environment.IsDevelopment()));
            services.AddMemoryCache();

            // Health checks
            services.AddHealthChecks()
                .AddDbContextCheck<DigNDB_SmittestopContext>("DB Smittestop", HealthStatus.Unhealthy, new[] {Startup.DatabaseTag})
                .AddCheck<HangFireHealthCheck>("HangFire", HealthStatus.Unhealthy, new[] {Startup.HangFireTag})
                .AddCheck<LogFilesHealthCheck>("Log files", HealthStatus.Unhealthy, new[] {Startup.LogFilesTag})
                //.AddCheck<ZipFilesHealthCheck>("Zip files", HealthStatus.Unhealthy, new[] {Startup.ZipFilesTag})
                .AddCheck<NumbersTodayHealthCheck>("Numbers today", HealthStatus.Unhealthy, new[] {Startup.NumbersTodayTag})
                .AddCheck<RollingStartNumberHealthCheck>("Rolling start number", HealthStatus.Unhealthy, new[] {Startup.RollingStartNumberTag});
                
            var connectionString = configuration["SQLConnectionString"];
            services.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseSqlServer(connectionString, x => x.MigrationsAssembly("DIGNDB.App.SmitteStop.DAL")));
            services.AddScoped<DigNDB_SmittestopContext>();

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(CountryMapper));
            services.AddAutoMapper(typeof(ApplicationStatisticsMapper));
            services.AddAutoMapper(typeof(StatisticsMapper));
            services.AddAutoMapper(typeof(CovidStatisticsMapper));

            services.AddAPIConfiguration(configuration);

            return services;
        }

        public static IServiceCollection AddApiServiceCollectionDependencies(this IServiceCollection services)
        {
            services.AddOptions();

            services.AddSingleton<IExposureConfigurationService, ExposureConfigurationService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICacheOperationsV3, CacheOperationsV3>();
            services.AddSingleton<IExposureConfigurationService, ExposureConfigurationService>();
            services.AddScoped<IAddTemporaryExposureKeyService, AddTemporaryExposureKeyService>();
            services.AddScoped<IExposureKeyValidator, ExposureKeyValidator>();
            services.AddScoped<IExposureKeyReader, ExposureKeyReader>();
            services.AddScoped<ILogMessageValidator, LogMessageValidator>();
            services.AddScoped<ICacheOperations, CacheOperations>();
            services.AddScoped<MobileAuthorizationAttribute>();
            services.AddScoped<GitHubAuthorizationAttribute>();
            services.AddScoped<DeprecatedCheckAttribute>();
            services.AddScoped<UploadKeysAuthorizationAttribute>();

            services.AddScoped<IJwtValidationService, JwtValidationService>();
            services.AddSingleton<IRsaProviderService, JwkRsaProviderService>();
            services.AddScoped<IJwtTokenReplyAttackService, JwtTokenReplyAttackService>();

            services.AddScoped<ISeedStore, AnonymousTokenSeedStore>();
            services.AddSingleton<IAnonymousTokenKeySource, AnonymousTokenKeySource>();
            services.AddScoped<IAnonymousTokenValidationService, AnonymousTokenValidationService>();

            services.AddScoped<IStatisticsFileService, StatisticsFileService>();

            return services;
        }

        private static IServiceCollection AddAPIConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var appsettingsConfig = configuration.GetSection("AppSettings").Get<AppSettingsConfig>();
            ModelValidator.ValidateContract(appsettingsConfig);

            var logValidationRulesConfig = configuration.GetSection("LogValidationRules").Get<LogValidationRulesConfig>();
            ModelValidator.ValidateContract(logValidationRulesConfig);

            var keyValidationRulesConfig = configuration.GetSection("KeyValidationRules").Get<KeyValidationConfiguration>();
            ModelValidator.ValidateContract(logValidationRulesConfig);

            services.AddSingleton(appsettingsConfig);
            services.AddSingleton<IOriginSpecificSettings>(appsettingsConfig);
            services.AddSingleton<IZipPackageBuilderConfig>(appsettingsConfig);
            services.AddSingleton(logValidationRulesConfig);
            services.AddSingleton(keyValidationRulesConfig);

            var jwtAuthorizationConfig = configuration.GetSection(nameof(JwtAuthorization)).Get<JwtAuthorization>();
            ModelValidator.ValidateContract(jwtAuthorizationConfig);
            services.AddSingleton(jwtAuthorizationConfig);

            services.AddSingleton(configuration);

            services.Configure<AnonymousTokenKeyStoreConfiguration>(configuration.GetSection("AnonymousTokenValidation"));

            return services;
        }
    }
}