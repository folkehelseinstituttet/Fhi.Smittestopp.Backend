using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.IntegrationTests
{
    [TestFixture]
    public class HealthCheckHangFireTest
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

        //[Test]
        //public async Task WhenSomeTextIsPosted_ThenTheResultIsOk()
        //{
        //    var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Backpack for his applesauce"));
        //    textContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        //    var result = await _client.PostAsync("/sample", textContent);
        //    Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //}

        //[Test]
        //public async Task WhenNoTextIsPosted_ThenTheResultIsBadRequest()
        //{
        //    var result = await _client.PostAsync("/sample", new StringContent(string.Empty));
        //    Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        //}

        [Test]
        public async Task HealthCheckHangFire_NoFailedJobs_Returns200()
        {
            // Arrange
            //var client = _factory.WithWebHostBuilder(builder =>
            //    {
            //        builder.ConfigureTestServices(services =>
            //        {
            //            services.AddAuthentication(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme).AddNoOperationAuthentication();

            //            //services.AddAuthentication(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)
            //            //    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
            //            //        "Test", options => { });
            //        });
            //    })
            //    .CreateClient(new WebApplicationFactoryClientOptions
            //    {
            //        AllowAutoRedirect = false,
            //    });

            //client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Test");

            //Act
            var response = await _client.GetAsync("/health/hangfire");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
