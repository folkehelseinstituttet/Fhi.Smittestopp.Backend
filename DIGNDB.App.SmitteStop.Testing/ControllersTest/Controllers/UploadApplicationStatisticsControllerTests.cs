using DIGNDB.App.SmitteStop.API.Controllers;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.Controllers
{
    [TestFixture]
    public class UploadApplicationStatisticsControllerTests
    {
        private MockRepository _mockRepository;
        private Mock<IApplicationStatisticsRepository> _mockApplicationStatisticsRepository;
        private Mock<ILogger<ApplicationStatisticsController>> _logger;

        private ApplicationStatisticsController _controller;

        [SetUp]
        public void Init()
        {
            SetupMockServices();

            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockApplicationStatisticsRepository = _mockRepository.Create<IApplicationStatisticsRepository>();

            _controller = new ApplicationStatisticsController(_mockApplicationStatisticsRepository.Object, _logger.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = MakeFakeContext(true).Object }
            };    
        }

        private void SetupMockServices()
        {
            _logger = new Mock<ILogger<ApplicationStatisticsController>>();

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
            var result = _controller.UpdateApplicationStatistics().Result;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            contextWithoutCacheControl.Verify(c => c.Request.Body, Times.Once);
            Assert.That(((BadRequestObjectResult)result).Value.ToString(), Does.StartWith("No application statistics found in body or unable to parse data"));
        }

        [Test]
        public void UpdateApplicationStatistics_Handles_NoAppStatsEntries()
        {
            //Arrange
            var applicationStatistics = new MemoryStream(Encoding.UTF8.GetBytes(AppStats));
            var contextWithoutCacheControl = MakeFakeContext(false);
            contextWithoutCacheControl.Setup(x => x.Request.Body).Returns(applicationStatistics);
            contextWithoutCacheControl.Setup(x => x.Request.ContentLength).Returns(applicationStatistics.Length);
            _controller.ControllerContext.HttpContext = contextWithoutCacheControl.Object;

            _mockApplicationStatisticsRepository.Setup(a => a.GetNewestEntryAsync())
                .Returns(Task.FromResult<ApplicationStatistics>(null));
            _mockApplicationStatisticsRepository.Setup(a => a.UpdateEntry(It.IsAny<ApplicationStatistics>()))
                .Verifiable();

            //Act
            var result = _controller.UpdateApplicationStatistics().Result;

            //Assert
            Assert.That(((ObjectResult) result).Value.ToString(), Is.EqualTo("Application statistics uploaded successfully"));
        }

        private const string AppStats = @"{
    ""PositiveResultsLast7Days"": 117,
    ""SmittestopDownloadsTotal"": 1003300,
    ""PositiveTestsResultsTotal"": 2978
}";
    }
}
