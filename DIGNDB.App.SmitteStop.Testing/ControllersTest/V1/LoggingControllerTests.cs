using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.API.Controllers;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V1
{
    [TestFixture]
    public class LoggingControllerTests
    {
        private Mock<ILogMessageValidator> _logMessageValidator;
        private Mock<IConfiguration> _configuration;
        private Mock<ILogger<LoggingController>> _logger;

        private LoggingController _controller;

        [SetUp]
        public void Init()
        {
            SetupMockServices();

            var appSettingsConfig = new AppSettingsConfig()
            {
                LogEndpointOverride = false
            };

            var logValidationRulesConfig = new LogValidationRulesConfig()
            {
                SeverityRegex = "^(ERROR|INFO|WARNING)$",
                PositiveNumbersRegex = "^[0-9]\\d*$",
                BuildVersionRegex = "^[1-9]{1}[0-9]*([.][0-9]*){1,2}?$",
                OperationSystemRegex = "^(IOS|Android-Google|Android-Huawei|Unknown)$",
                DeviceOSVersionRegex = "[1-9]{1}[0-9]{0,2}([.][0-9]{1,3}){1,2}?$",
                MaxTextFieldLength = 500,
            };

            _controller = new LoggingController(_logMessageValidator.Object, _logger.Object, logValidationRulesConfig, appSettingsConfig)

            {
                ControllerContext = new ControllerContext() { HttpContext = MakeFakeContext(true).Object }
            };    
        }

        private void SetupMockServices()
        {
            _logger = new Mock<ILogger<LoggingController>>();
            _logMessageValidator = new Mock<ILogMessageValidator>(MockBehavior.Strict);
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);

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
