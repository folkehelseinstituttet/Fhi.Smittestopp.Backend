using DIGNDB.App.SmitteStop.Core.Contracts;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FederationGatewayApi
{
    public static class ContainerRegistration
    {
        public static IServiceCollection AddGatewayDependencies(this IServiceCollection services)
        {
            services.AddScoped<IKeyFilter, KeyFilter>();
            services.AddScoped<IEuGatewayService, EuGatewayService>();
            services.AddScoped<IGatewayWebContextReader, GatewayWebContextReader>();
            services.AddScoped<IGatewayHttpClient, GatewayHttpClient>();
            services.AddScoped<IEFGSKeyStoreService, EFGSKeyStoreService>();
            services.AddScoped<IDaysSinceOnsetOfSymptomsDecoder, DaysSinceOnsetOfSymptomsDecoder>();
            services.AddScoped<ISignatureService, SignatureService>();

            return services;
        }
    }
}