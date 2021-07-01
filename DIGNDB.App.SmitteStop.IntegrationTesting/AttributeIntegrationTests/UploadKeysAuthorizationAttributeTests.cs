using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.IntegrationTesting.Mocks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.AttributeIntegrationTests
{
    [TestFixture]
    public class UploadKeysAuthorizationAttributeTests : IntegrationTest
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
        public async Task UploadKeys_InvalidAnonymousToken_ReturnsUnauthorized()
        {
            // Arrange
            var appSettings = new Dictionary<string, string>();
            InitiateClient(appSettings, AddServices);

            var content = new System.Net.Http.StringContent(@"{
    ""keys"": [
            {
                ""daysSinceOnsetOfSymptoms"": 0,
                ""key"": ""ch/71n3jkWz99HTQsgBfOw=="",
                ""rollingStart"": ""2021-05-05T00:00:00+00:00"",
                ""rollingDuration"": ""1.00:00:00"",
                ""transmissionRiskLevel"": 4
            }
            ],
            ""regions"": [
            ""no""
                ],
            ""visitedCountries"": [""SE""],
            ""appPackageName"": ""test"",
            ""platform"": ""iOS"",
            ""padding"": """"
        }");

            //Act
            const int apiVersion = 3;
            var url = $"api/v{apiVersion}/diagnostickeys";
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Anonymous","InvalidToken");
            var response = await Client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private static int AddServices(IServiceCollection services)
        {
            var jwtValidationServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IJwtValidationService));
            services.Remove(jwtValidationServiceDescriptor);
            services.AddScoped<IJwtValidationService, JwtValidationServiceMock>(x => new JwtValidationServiceMock(false));

            var anonymousTokenValidationServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAnonymousTokenValidationService));
            services.Remove(anonymousTokenValidationServiceDescriptor);
            services.AddScoped<IAnonymousTokenValidationService, AnonymousValidationServiceMock>(x => new AnonymousValidationServiceMock(false));

            return 0;
        }
    }
}
