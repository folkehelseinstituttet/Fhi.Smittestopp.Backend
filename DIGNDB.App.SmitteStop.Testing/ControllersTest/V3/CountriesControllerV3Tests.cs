using AutoMapper;
using DIGNDB.App.SmitteStop.API.V3.Controllers;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V3
{

    [TestFixture]
    public class CountriesControllerV3Tests
    {
        private Mock<ICountryService> _countryService;
        private Mock<ILogger<CountriesControllerV3>> _logger;
        private Mock<IMapper> _mapper;

        private CountriesControllerV3 _controller;

        private IEnumerable<Country> _exampleCountries;

        [SetUp]
        public void Init()
        {
            SetupMockServices();
            _controller = new CountriesControllerV3(_countryService.Object, _logger.Object, _mapper.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = MakeFakeContext(true).Object }
            };
        }

        private void SetupMockServices()
        {
            _exampleCountries = new List<Country>
            {
                new Country
                {
                    Code = "PL",
                    Id = 1,
                    PullingFromGatewayEnabled = true
                },
                new Country
                {
                    Code = "SE",
                    Id = 2,
                    PullingFromGatewayEnabled = true
                }
            };

            _logger = new Mock<ILogger<CountriesControllerV3>>();

            _mapper = new Mock<IMapper>();

            _countryService = new Mock<ICountryService>(MockBehavior.Strict);
            _countryService
                .Setup(mock => mock.GetAllCountries())
                .ReturnsAsync(_exampleCountries);
            _countryService
                .Setup(mock => mock.GetVisibleCountries(It.IsAny<string>()))
                .ReturnsAsync(_exampleCountries);
        }

        private Mock<HttpContext> MakeFakeContext(bool hasCacheControl)
        {
            var mockRequest = new Mock<HttpRequest>();
            var requestHeader = new Mock<HeaderDictionary>();
            var mockResponse = new Mock<HttpResponse>();
            var responseHeader = new Mock<HeaderDictionary>();
            if (hasCacheControl)
            {
                requestHeader.Object.Add("Cache-Control", "no-cache");
            }
            mockRequest.Setup(res => res.Headers).Returns(requestHeader.Object);
            mockResponse.Setup(res => res.Headers).Returns(responseHeader.Object);
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            return mockContext;
        }

        [Test]
        public void TestGetAllCountries()
        {
            var result = _controller.GetAllCountries("DE");

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void TestGetAllCountries_NoParameterGiven()
        {
            var result = _controller.GetAllCountries();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
