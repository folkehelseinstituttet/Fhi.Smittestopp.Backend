﻿using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using DIGNDB.App.SmitteStop.IntegrationTesting.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    public enum HealthStatus
    {
        Healthy,
        Unhealthy
    };

    public class HealthCheckTests
    {
        public WebApplicationFactory<Startup> Factory;
        public HttpClient Client;

        /// <summary>
        /// Initiates _client using passed appSettings 
        /// </summary>
        /// <param name="appSettings">Overwrites configuration values, see API/appsetting.json</param>
        public void InitiateClient(Dictionary<string, string> appSettings)
        {
            Client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(FileSystem));
                    services.Remove(descriptor);

                    services.AddScoped<IFileSystem, FileSystemMock>();
                    services.AddScoped<IPathHelper, PathHelperMock>();
                });
                builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var c = configBuilder.AddInMemoryCollection(appSettings);

                });
            })
                .CreateClient();
        }

        public static async Task<T> HealthCheckResultReadAsStreamAsync<T>(HttpResponseMessage response, JsonSerializerOptions options)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            var health = await JsonSerializer.DeserializeAsync<T>(contentStream, options);

            Assert.NotNull(health);

            return health;
        }

        public static async Task<HealthCheckStatusResult> HealthCheckResultReadAsStringAsync<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var health = JsonSerializer.Deserialize<HealthCheckStatusResult>(result);

            Assert.NotNull(health);

            return health;
        }
    }
}
