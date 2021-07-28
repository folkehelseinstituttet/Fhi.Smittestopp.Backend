using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DIGNDB.App.SmitteStop.API.V3.Controllers;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.ControllerIntegrationTests
{

    
    public class CoutriesControllerIntegrationTest : IntegrationTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            InitializeFactory();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DisposeClientAndFactory();
        }

        [Test]
        public async Task CountryController_Test1_ReturnsOk()
        {
            // Arrange
            var machineName = Environment.MachineName.ToLower();
            var appSettings = new Dictionary<string, string>();

            InitiateClient(appSettings, AddServices);

            //Act
            var response = await Client.GetAsync($"/api/v3/countries");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var healthCheckResult = ReadJsonFromResponse<HealthCheckResult>(response);
            var result = healthCheckResult.Result;

        }
        private static int AddServices(IServiceCollection services)
        {
            var fileSystemDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICountryRepository));
            services.Remove(fileSystemDescriptor);
            services.AddScoped<ICountryRepository, CountryRepositoryMock>();

            fileSystemDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICountryService));
            services.Remove(fileSystemDescriptor);
            services.AddScoped<ICountryService, CountryServiceMock>();

            return 1;
        }
    }
}


