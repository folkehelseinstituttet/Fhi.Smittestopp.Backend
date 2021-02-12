using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using FederationGatewayApi.Services.Settings;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.EventLog;
using System.Reflection;

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
            services.AddSingleton(hangfireConfig.Jobs.DownloadKeysFromTheGateway);

            services.AddHangfire(x => x.UseSqlServerStorage(hangfireConfig.HangFireConnectionString));
            services.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseSqlServer(hangfireConfig.SmittestopConnectionString));

            services.AddControllers().AddControllersAsServices();
            services.AddLogging();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(CountryMapper));

            services.AddScoped<IDatabaseKeysValidationService, DatabaseKeysValidationService>();
            services.AddScoped<IZipFileService, ZipFileService>();
            services.AddScoped<ISettingsService, SettingsService>();

            services.AddSingleton(hangfireConfig);
            services.AddSingleton<IOriginSpecificSettings>(hangfireConfig);
            services.AddSingleton<IZipPackageBuilderConfig>(hangfireConfig);
            services.AddScoped<IZipFileService, ZipFileService>();

            services.AddScoped<IGatewayHttpClient, GatewayHttpClient>();
            services.AddScoped<ITemporaryExposureKeyRepository, TemporaryExposureKeyRepository>();
            services.AddScoped<ISignatureService, SignatureService>();
            services.AddScoped<IEncodingService, EncodingService>();
            services.AddScoped<IGatewayWebContextReader, GatewayWebContextReader>();
            services.AddScoped<IEpochConverter, EpochConverter>();
            services.AddScoped<IEFGSKeyStoreService, EFGSKeyStoreService>();
            services.AddScoped<IGatewayKeyProvider, GatewayKeyProvider>();
            services.AddScoped<IRiskCalculator, RiskCalculator>();
            services.AddScoped<IDaysSinceOnsetOfSymptomsDecoder, DaysSinceOnsetOfSymptomsDecoder>();
            services.AddScoped<IKeyFilter, KeyFilter>();
            services.AddScoped<IGatewaySyncStateSettingsDao, GatewaySyncStateSettingsDao>();
            services.AddScoped<IEuGatewayService, EuGatewayService>();
            services.AddScoped<IAddTemporaryExposureKeyService, AddTemporaryExposureKeyService>();
            services.AddSingleton<IGatewayKeyProvider>(
                new GatewayKeyProvider(gateWayConfig.AuthenticationCertificateFingerprint, gateWayConfig.SigningCertificateFingerprint));


            return services;
        }

    }
}