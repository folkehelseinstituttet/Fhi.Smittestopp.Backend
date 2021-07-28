using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.IntegrationTesting.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.IntegrationTesting
{
    public enum HealthStatus
    {
        Healthy,
        Unhealthy
    }

    public class IntegrationTest
    {
        private WebApplicationFactory<Startup> _factory;
        public HttpClient Client;

        public void InitializeFactory()
        {
            _factory = new WebApplicationFactory<API.Startup>();
        }

        public void DisposeClientAndFactory()
        {
            Client.Dispose();
            _factory.Dispose();
        }

        /// <summary>
        /// Initiates _client using passed appSettings 
        /// </summary>
        /// <param name="appSettings">Overwrites configuration values, see API/appsetting.json</param>
        /// <param name="addServices"></param>
        public void InitiateClient(Dictionary<string, string> appSettings, Func<IServiceCollection, int> addServices)
        {
            Client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services => { addServices(services); });

                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        var c = configBuilder.AddInMemoryCollection(appSettings);

                    });
                })
                .CreateClient();
        }

        #region HealthCheck test helpers

        public static async Task<HealthCheckStatusResult> HealthCheckResultReadAsStringAsync<T>(
            HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var health = JsonSerializer.Deserialize<HealthCheckStatusResult>(result);

            Assert.NotNull(health);

            return health;
        }

        public static async Task<CountryCollectionDto> ReadJsonFromResponse<T>(
            HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var health = JsonSerializer.Deserialize<CountryCollectionDto>(result);

            //Assert.NotNull(health);

            return health;
        }

        #endregion
    }
}
