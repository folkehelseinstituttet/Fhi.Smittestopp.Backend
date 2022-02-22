using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.API.V3.Controllers;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V3
{
    public class ImportantInfoControllerV3Tests
    {
        [TestFixture]
        public class CountriesControllerV3Tests
        {
            private Mock<IImportantInfoService> _importantInfoService;
            private Mock<ILogger<ImportantInfoControllerV3>> _logger;

            private ImportantInfoControllerV3 _controller;

            [SetUp]
            public void Init()
            {
                _logger = new Mock<ILogger<ImportantInfoControllerV3>>();
                _importantInfoService = new Mock<IImportantInfoService>(MockBehavior.Strict);
                _controller = new ImportantInfoControllerV3(_logger.Object, _importantInfoService.Object);
            }

            [Test]
            public async Task TestGetImportantInfo_InfoInLanguageAvailable_ReturnsInfo()
            {
                string infoText = "Important";
                MockServiceResponses(true, new ImportantInfoResponse() { text = infoText });

                IActionResult result = await _controller.GetImportantInfo("NB");

                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                OkObjectResult okResult = result as OkObjectResult;

                Assert.That(okResult.Value, Is.InstanceOf<ImportantInfoResponse>());
                ImportantInfoResponse infoResponse = okResult.Value as ImportantInfoResponse;

                Assert.AreEqual(infoText, infoResponse.text);
                _importantInfoService.Verify(mock => mock.ConfigFileExists(), Times.Once);
                _importantInfoService.Verify(mock => mock.ParseConfig(It.IsAny<ImportantInfoRequest>()), Times.Once);
            }

            [Test]
            public async Task TestGetImportantInfo_InfoInLanguageNotAvailable_ReturnsNoContent()
            {
                MockServiceResponses(true, new ImportantInfoResponse() { text = null });

                IActionResult result = await _controller.GetImportantInfo("NB");

                Assert.That(result, Is.InstanceOf<NoContentResult>());
                _importantInfoService.Verify(mock => mock.ConfigFileExists(), Times.Once);
                _importantInfoService.Verify(mock => mock.ParseConfig(It.IsAny<ImportantInfoRequest>()), Times.Once);
            }

            [Test]
            public async Task TestGetImportantInfo_FileNotExist_ReturnsNoContent()
            {
                MockServiceResponses(false, new ImportantInfoResponse() { text = "important" });

                IActionResult result = await _controller.GetImportantInfo("NB");

                Assert.That(result, Is.InstanceOf<NoContentResult>());
                _importantInfoService.Verify(mock => mock.ConfigFileExists(), Times.Once);
                _importantInfoService.Verify(mock => mock.ParseConfig(It.IsAny<ImportantInfoRequest>()), Times.Never);
            }

            private void MockServiceResponses(bool fileExists, ImportantInfoResponse response)
            {
                _importantInfoService
                   .Setup(mock => mock.ConfigFileExists())
                   .Returns(fileExists);
                _importantInfoService
                    .Setup(mock => mock.ParseConfig(It.IsAny<ImportantInfoRequest>()))
                    .Returns(response);
            }
        }
    }
}
