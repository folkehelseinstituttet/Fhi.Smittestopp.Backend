using AutoMapper;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Text.Json;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class WebContextReaderTests
    {
        private IGatewayWebContextReader WebContextReader { get; set; }
        private WebContextMock WebContextMock { get; set; }

        [SetUp]
        public void Init()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(EuGatewayProtoToDtosMapper));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var autoMapper = serviceProvider.GetService<IMapper>();
            var loggerGatewayWebContextReader = new Mock<ILogger<GatewayWebContextReader>>();
            WebContextReader = new GatewayWebContextReader(autoMapper, loggerGatewayWebContextReader.Object);
            WebContextMock = new WebContextMock();
        }

        [Test]
        public void ReaderReadsObjectsBasedOnJson()
        {
            var mockResponseBody = WebContextMock.MockValidBodyJson();
            var keys = WebContextReader.GetItemsFromRequest(mockResponseBody);
            Assert.That(keys.Count > 0);
        }

        [Test]
        public void ReaderReadsObjectsBasedOnInvalidJson()
        {
            var mockResponseBody = WebContextMock.MockInvalidBodyJson();

            Assert.Throws<JsonException>(() => WebContextReader.GetItemsFromRequest(mockResponseBody));
        }

        [Test]
        public void ReaderReadsObjectsBasedOnJsonForNoBatches()
        {
            var mockResponseBody = WebContextMock.MockNoBatchesBodyJson();
            var keys = WebContextReader.GetItemsFromRequest(mockResponseBody);

            Assert.That(keys.Count == 0);
        }

        [Test]
        public void ReaderReadsTheJsonContentFromHttpRequest()
        {
            var mockResponse = WebContextMock.MockHttpResponse();
            var response = WebContextReader.ReadHttpContextStream(mockResponse);
            Assert.AreEqual(response, WebContextMock.MockValidBodyJson());
        }
    }
}
