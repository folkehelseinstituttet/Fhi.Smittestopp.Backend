using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DIGNDB.App.SmitteStop.API;
using Moq;
using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.Extensions.Logging;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using DIGNDB.App.SmitteStop.Core.Models;
using Microsoft.AspNetCore.Http;
using DIGNDB.App.SmitteStop.API.Services;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using AutoMapper;
using DIGNDB.App.SmitteStop.API.V2.Controllers;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V2
{

    [TestFixture]
    public class CountriesControllerTests
    {
        private Mock<ICountryService> _countryService;
        private Mock<ILogger<CountriesController>> _logger;
        private Mock<IMapper> _mapper;

        private CountriesController _controller;

        private IEnumerable<Country> _exampleCountries;

        [SetUp]
        public void Init()
        {
            SetupMockServices();
            _controller = new CountriesController(_countryService.Object, _logger.Object, _mapper.Object)
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

            _logger = new Mock<ILogger<CountriesController>>();

            _mapper = new Mock<IMapper>();

            _countryService = new Mock<ICountryService>(MockBehavior.Strict);
            _countryService
                .Setup(mock => mock.GetAllCountries())
                .ReturnsAsync(_exampleCountries);
            _countryService
                .Setup(mock => mock.GetVisibleCountries())
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
            var result = _controller.GetAllCountries();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
