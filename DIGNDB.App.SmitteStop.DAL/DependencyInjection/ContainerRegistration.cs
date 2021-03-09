using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DIGNDB.App.SmitteStop.DAL.DependencyInjection
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ITemporaryExposureKeyRepository), typeof(TemporaryExposureKeyRepository));
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped(typeof(ICovidStatisticsRepository), typeof(CovidStatisticsRepository));
            services.AddScoped(typeof(IApplicationStatisticsRepository), typeof(ApplicationStatisticsRepository));
            services.AddScoped<ITemporaryExposureKeyRepository, TemporaryExposureKeyRepository>();

            return services;
        }
    }
}