using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    [TestFixture]
    public class HealthCheckHangFireTest : IntegrationTest
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
            var appSettings = new Dictionary<string, string>();

            InitiateClient(appSettings, AddServices);

            //Act
            var response = await Client.GetAsync("/health/hangfire");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static int AddServices(IServiceCollection services)
        {
            var hangFireServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHealthCheckHangFireService));
            services.Remove(hangFireServiceDescriptor);
            services.AddScoped<IHealthCheckHangFireService, HealthCheckHangFireServiceMock>(x =>
            {
                long failedCount = 0;
                int serversCount = 1;
                int noOfRecurringJobs = 5;
                return new HealthCheckHangFireServiceMock(failedCount, serversCount, noOfRecurringJobs);
            });

            return 0;
        }
    }
}
