using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DIGNDB.App.SmitteStop.DAL.DependencyInjection
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();

            return services;
        }
    }
}