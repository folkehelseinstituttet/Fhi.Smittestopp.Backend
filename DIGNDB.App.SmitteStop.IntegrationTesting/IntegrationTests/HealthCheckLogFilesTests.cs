using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    [TestFixture]
    public class HealthCheckLogFilesTests : HealthCheckTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            Factory = new WebApplicationFactory<Startup>();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Client.Dispose();
            Factory.Dispose();
        }

        [SetUp]
        public void EnsureClient()
        {
            InitiateClient(new Dictionary<string, string>());
        }

        [Test]
        public async Task HealthCheckLogFiles_DirectoriesDoesNotExist_ReturnsUnhealthy()
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
            var response = await Client.GetAsync($"/health/logfiles?server={machineName}");

            // Assert
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            var healthCheckResult = HealthCheckResultReadAsStringAsync<HealthCheckResult>(response);
            var result = healthCheckResult.Result;

            Assert.AreEqual(HealthStatus.Unhealthy.ToString(), result.status);
            Assert.AreEqual(3, result.results.data.Count);
        }

        [Test]
        public async Task HealthCheckLogFiles_DirectoriesDoesExist_ReturnsHealthy()
        {
            // Arrange
            var machineName = Environment.MachineName.ToLower();
            var appSettings = new Dictionary<string, string>
            {
                ["AppSettings:LogsApiPath"] = $"ApiLogs{Constants.DoesExist}",
                ["AppSettings:LogsJobsPath"] = $"JobsLogs{Constants.DoesExist}",
                ["AppSettings:LogsMobilePath"] = $"MobileLogs{Constants.DoesExist}",
                ["HealthCheckSettings:Server1Name"] = machineName
            };

            InitiateClient(appSettings);

            //Act
            var response = await Client.GetAsync($"/health/logfiles?server={machineName}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var healthCheckResult = HealthCheckResultReadAsStringAsync<HealthCheckResult>(response);
            var result = healthCheckResult.Result;

            Assert.AreEqual(HealthStatus.Healthy.ToString(), result.status);
            Assert.AreEqual(0, result.results.data.Count);
        }
    }
}
