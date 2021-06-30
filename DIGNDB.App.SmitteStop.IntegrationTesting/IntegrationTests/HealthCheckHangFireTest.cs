using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    [TestFixture]
    public class HealthCheckHangFireTest : HealthCheckTests
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
            var response = await Client.GetAsync("/health/hangfire");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
