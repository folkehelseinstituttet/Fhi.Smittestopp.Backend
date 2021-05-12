using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
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
            var hangFireConfig = configuration.Get<HangfireConfig>();
            ModelValidator.ValidateContract(hangFireConfig);
            
            int DelayInSecondsByAttemptFunc(long attempt) => hangFireConfig.JobsRetryInterval;
            GlobalJobFilters.Filters.Add(
                new AutomaticRetryAttribute
                {
                    Attempts = hangFireConfig.JobsMaxRetryAttempts, 
                    DelayInSecondsByAttemptFunc = DelayInSecondsByAttemptFunc
                });

            var gateWayConfig = hangFireConfig.EuGateway;
            var eventLogConfig = hangFireConfig.Logging.EventLog;
            services.Configure<EventLogSettings>(config =>
            {
                config.SourceName = eventLogConfig.SourceName;
                config.LogName = eventLogConfig.LogName;
                config.MachineName = eventLogConfig.MachineName;
            });
            services.AddSingleton(gateWayConfig);
            services.AddSingleton(hangFireConfig.Jobs.UploadKeysToTheGateway);
            services.AddSingleton(hangFireConfig.Jobs.DownloadKeysFromTheGateway);
            services.AddSingleton(hangFireConfig.Jobs.GetCovidStatistics);

            services.AddHangfire(x => x.UseSqlServerStorage(hangFireConfig.HangFireConnectionString));
            services.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseSqlServer(hangFireConfig.SmittestopConnectionString));

            services.AddControllers().AddControllersAsServices();
            services.AddLogging();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(CountryMapper));

            services.AddScoped<IDatabaseKeysValidationService, DatabaseKeysValidationService>();
            services.AddScoped<IZipFileService, ZipFileService>();
            services.AddScoped<ISettingsService, SettingsService>();

            services.AddSingleton(hangFireConfig);
            services.AddSingleton<IOriginSpecificSettings>(hangFireConfig);
            services.AddSingleton<IZipPackageBuilderConfig>(hangFireConfig);
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
            services.AddScoped<IFetchCovidStatisticsService, FetchCovidStatisticsService>();
            services.AddScoped<ICovidStatisticsFilePackageBuilder, CovidStatisticsFilePackageBuilder>();
            services.AddScoped<IDateTimeResolver, DateTimeResolver>();
            services.AddScoped<ICovidStatisticsDataExtractingService, CovidStatisticsDataExtractingService>();
            services.AddScoped<ICovidStatisticsCsvParser, CovidStatisticsCsvParser>();
            services.AddScoped<ICovidStatisticsBuilder, CovidStatisticsBuilder>();
            services.AddScoped<ICovidStatisticsRetrieveService, CovidStatisticsRetrieveService>();
            services.AddScoped<ICovidStatisticsCsvDataRetrieveService, CovidStatisticsCsvDataRetrieveService>();
            services.AddSingleton<IGatewayKeyProvider>(
                new GatewayKeyProvider(gateWayConfig.AuthenticationCertificateFingerprint, gateWayConfig.SigningCertificateFingerprint));


            return services;
        }

    }
}