using DIGNDB.App.SmitteStop.Core.Contracts;
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

            return services;
        }
    }
}