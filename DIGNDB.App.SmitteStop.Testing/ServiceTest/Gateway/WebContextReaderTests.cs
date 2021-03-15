using AutoMapper;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

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
            WebContextReader = new GatewayWebContextReader(autoMapper);
            WebContextMock = new WebContextMock();
        }

        [Test]
        public void ReaderReadsObjectsBasedOnJSON()
        {
            var mockResponseBody = WebContextMock.MockValidBodyJSON();
            var keys = WebContextReader.GetItemsFromRequest(mockResponseBody);
            Assert.That(keys.Count > 0);
        }

        [Test]
        public void ReaderReadsTheJsonContentFromHttpRequest()
        {
            var mockResponse = WebContextMock.MockHttpResponse();
            var response = WebContextReader.ReadHttpContextStream(mockResponse);
            Assert.AreEqual(response, WebContextMock.MockValidBodyJSON());
        }
    }
}
