using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V2
{
    [TestFixture]
    public class LoggingControllerV2Tests
    {
        private Mock<ILogMessageValidator> _logMessageValidator;
        private Mock<IConfiguration> _configuration;
        private Mock<ILogger<LoggingControllerV2>> _logger;

        private LoggingControllerV2 _controller;

        [SetUp]
        public void Init()
        {
            SetupMockServices();
            SetupMockConfiguration();
            _controller = new LoggingControllerV2(_logMessageValidator.Object, _logger.Object, _configuration.Object)

            {
                ControllerContext = new ControllerContext() { HttpContext = MakeFakeContext(true).Object }
            };
            

        }

        private void SetupMockServices()
        {
            _logger = new Mock<ILogger<LoggingControllerV2>>();
            _logMessageValidator = new Mock<ILogMessageValidator>(MockBehavior.Strict);
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);

        }

        private void SetupMockConfiguration()
        {
            _configuration.Setup(config => config["LogValidationRules:severityRegex"]).Returns("^(ERROR|INFO|WARNING)$");
            _configuration.Setup(config => config["LogValidationRules:positiveNumbersRegex"]).Returns("^[0-9]\\d*$");
            _configuration.Setup(config => config["LogValidationRules:buildVersionRegex"]).Returns("^[1-9]{1}[0-9]*([.][0-9]*){1,2}?$");
            _configuration.Setup(config => config["LogValidationRules:operationSystemRegex"]).Returns("^(IOS|Android-Google|Android-Huawei|Unknown)$");
            _configuration.Setup(config => config["LogValidationRules:deviceOSVersionRegex"]).Returns("[1-9]{1}[0-9]{0,2}([.][0-9]{1,3}){1,2}?$");
            _configuration.Setup(config => config["LogValidationRules:maxTextFieldLength"]).Returns("500");
            _configuration.Setup(config => config["AppSettings:logEndpointOverride"]).Returns("false");

        }

        public Mock<HttpContext> MakeFakeContext(bool hasCacheControl)
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
        public void ReturnBadRequest_When_RequestBodyIsNotParsable()
        {
            //Arrange
            var badRequestJsonBodyStream = new MemoryStream(Encoding.UTF8.GetBytes("attribute :**? value"));
            var contextWithoutCacheControl = MakeFakeContext(false);
            contextWithoutCacheControl.Setup(x => x.Request.Body).Returns(badRequestJsonBodyStream);
            contextWithoutCacheControl.Setup(x => x.Request.ContentLength).Returns(badRequestJsonBodyStream.Length);
            _controller.ControllerContext.HttpContext = contextWithoutCacheControl.Object;

            //Act
            var result = _controller.UploadMobileLogs().Result;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            contextWithoutCacheControl.Verify(c => c.Request.Body, Times.Once);
            Assert.That(((BadRequestObjectResult)result).Value.ToString(), Does.StartWith("No logs found in body or unable to parse logs data"));
        }
    }
}
