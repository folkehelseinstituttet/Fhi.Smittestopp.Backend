using System.Reflection;
using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.EventLog;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddJobsDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireConfig = configuration.Get<HangfireConfig>();
            ModelValidator.ValidateContract(hangfireConfig);
            var gateWayConfig = hangfireConfig.EuGateway;
            var eventLogConfig = hangfireConfig.Logging.EventLog;
            services.Configure<EventLogSettings>(config =>
            {
                config.SourceName = eventLogConfig.SourceName;
                config.LogName = eventLogConfig.LogName;
                config.MachineName = eventLogConfig.MachineName;
            });
            services.AddSingleton(gateWayConfig);
            services.AddSingleton(hangfireConfig.Jobs.UploadKeysToTheGateway);

            services.AddSingleton<IGatewayKeyProvider>(
                new GatewayKeyProvider(gateWayConfig.AuthenticationCertificateFingerprint, gateWayConfig.SigningCertificateFingerprint));

            services.AddHangfire(x => x.UseSqlServerStorage(hangfireConfig.HangFireConnectionString));
            services.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseSqlServer(hangfireConfig.SmittestopConnectionString));

            services.AddControllers().AddControllersAsServices();
            services.AddLogging();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(CountryMapper));

            services.AddScoped<IGatewayWebContextReader, GatewayWebContextReader>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IDatabaseKeysValidationService, DatabaseKeysValidationService>();
            services.AddScoped<IZipFileService, ZipFileService>();

            return services;
        }
    }
}