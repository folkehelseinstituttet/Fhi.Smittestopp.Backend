using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DIGNDB.App.SmitteStop.Core.DependencyInjection
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJwtValidationService, JwtValidationService>();
            services.AddSingleton<IRsaProviderService, JwkRsaProviderService>();
            services.AddScoped<IJwtTokenReplyAttackService, JwtTokenReplyAttackService>();
            services.AddScoped<IKeyValidator, KeyValidator>();
            services.AddScoped<IRiskCalculator, RiskCalculator>();
            services.AddScoped<IEpochConverter, EpochConverter>();
            services.AddScoped<IZipFileInfoService, ZipFileInfoService>();
            services.AddScoped<IFileSystem, FileSystem>();
            services.AddScoped(typeof(IDataAccessLoggingService<>), typeof(DataAccessLoggingService<>));
            services.AddScoped<IExposureKeyMapper, ExposureKeyMapper>();
            services.AddScoped<IPackageBuilderService, PackageBuilderService>();
            services.AddScoped<IKeysListToMemoryStreamConverter, KeysListToMemoryStreamConverter>();
            services.AddScoped<IDatabaseKeysToBinaryStreamMapperService, DatabaseKeysToBinaryStreamMapperService>();
            services.AddScoped<IDateTimeNowWrapper, DateTimeNowWrapper>();
            services.AddScoped<IEncodingService, EncodingService>();

            return services;
        }
    }
}