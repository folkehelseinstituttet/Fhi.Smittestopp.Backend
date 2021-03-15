using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.CovidStatisticsFilesTests.Services
{
    public class KeyNumbersWebServiceTests
    {
        private const string ExampleUrl = "http://localhost:8080/keynumbers/hospital";

        [Test]
        public void TestGetAsync()
        {
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };

            var keyNumbersWebService = new Mock<IWebServiceWrapper>();
            keyNumbersWebService.Setup((service) => service.GetAsync(ExampleUrl))
                .Returns(Task.FromResult(expectedResponse));

            var response = keyNumbersWebService.Object.GetAsync(ExampleUrl).Result;

            Assert.AreEqual(expectedResponse.StatusCode, response.StatusCode);
        }
    }
}
