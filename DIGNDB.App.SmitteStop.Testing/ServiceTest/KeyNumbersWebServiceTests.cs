using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
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

            var keyNumbersWebService = new Mock<IKeyNumbersWebService>();
            keyNumbersWebService.Setup((service) => service.GetAsync(ExampleUrl))
                .Returns(Task.FromResult(expectedResponse));

            var response = keyNumbersWebService.Object.GetAsync(ExampleUrl).Result;

            Assert.AreEqual(expectedResponse.StatusCode, response.StatusCode);
        }
    }
}
