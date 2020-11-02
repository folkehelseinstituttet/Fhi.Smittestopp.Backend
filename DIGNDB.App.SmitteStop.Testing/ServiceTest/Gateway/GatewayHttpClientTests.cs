using FederationGatewayApi.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class GatewayHttpClientTests
    {
        private const string FingerprintHeader = "X-SSL-Client-SHA256";
        private const string DistinctiveNameHeader = "X-SSL-Client-DN";

        private const string ExampleFingerprint = "ECD419378340C29A16C9B6CC5168F34A13B3D91B9E2E2C32D9B239EEE19A3AAD";
        private const string DkDistinctiveNameHeader = "C=DK";
        private const string ExampleUrl = "http://localhost:8080/diagnosiskeys/callback";

        [Test]
        public void TestGetAsync()
        {
            var handlerMock = new Mock<HttpClientHandler>();
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };

            Expression<Func<HttpRequestMessage, bool>> requestIsValid = message =>
                message.Headers
                    .Any(pair => pair.Key == DistinctiveNameHeader &&
                                 pair.Value.First() == DkDistinctiveNameHeader) &&
                message.Headers
                    .Any(pair => pair.Key == FingerprintHeader &&
                                 pair.Value.First() == ExampleFingerprint)
                && message.RequestUri == new Uri(ExampleUrl);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(requestIsValid),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var gatewayKeyProviderMock = new Mock<IGatewayKeyProvider>();
            gatewayKeyProviderMock.Setup(p => p.AuthenticationCertificate).Returns(new X509Certificate2());
            gatewayKeyProviderMock.Setup(p => p.AuthenticationCertificateFingerprint).Returns(ExampleFingerprint);


            using var gatewayHttpClient = new GatewayHttpClient(gatewayKeyProviderMock.Object, handlerMock.Object);
            var response = gatewayHttpClient.GetAsync(ExampleUrl).Result;

            Assert.AreEqual(expectedResponse.StatusCode, response.StatusCode);
        }
    }
}