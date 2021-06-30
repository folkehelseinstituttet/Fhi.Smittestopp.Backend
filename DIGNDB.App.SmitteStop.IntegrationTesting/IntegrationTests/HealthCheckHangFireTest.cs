using System;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    [TestFixture]
    public class HealthCheckHangFireTest : HealthCheckTests
    {
        private WebApplicationFactory<API.Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void Init()
        {
            _factory = new WebApplicationFactory<API.Startup>();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task HealthCheckHangFire_NoFailedJobs_ReturnsHealthy()
        {
            // Arrange
            var machineName = Environment.MachineName.ToLower();
            var appSettings = new Dictionary<string, string>
            {
                ["AppSettings:LogsApiPath"] = $"ApiLogs{Constants.DoesNotExist}",
                ["AppSettings:LogsJobsPath"] = $"JobsLogs{Constants.DoesNotExist}",
                ["AppSettings:LogsMobilePath"] = $"MobileLogs{Constants.DoesNotExist}",
                ["HealthCheckSettings:Server1Name"] = machineName
            };

            InitiateClient(appSettings);

            //Act
            var response = await _client.GetAsync("/health/hangfire");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
