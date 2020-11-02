using System;
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
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V2
{

    [TestFixture]
    public class DiagnosticKeysControllerV2Tests
    {
        private Mock<IAppleService> _appleService;
        private Mock<IConfiguration> _configuration;
        private Mock<IExposureKeyValidator> _exposureKeyValidator;
        private Mock<ILogger<DiagnosticKeysControllerV2>> _logger;
        private Mock<IExposureConfigurationService> _exposureConfigurationService;
        private Mock<IKeyValidationConfigurationService> _keyValidationConfigurationService;
        private Mock<IAddTemporaryExposureKeyService> _addTemporaryExposureService;
        private DiagnosticKeysControllerV2 _controller;
        private Mock<IZipFileInfoService> _zipFileInfoService;
        private Mock<IAppSettingsConfig> _appSettingsConfigMock;

        private static readonly byte[] fileContent = new byte[] { 1, 2, 3, 4, 5 };

        [SetUp]
        public void Init()
        {
            SetupMockServices();
            _controller = new DiagnosticKeysControllerV2(_logger.Object, _appleService.Object, _configuration.Object,
                _exposureKeyValidator.Object, _exposureConfigurationService.Object,
                _keyValidationConfigurationService.Object, _addTemporaryExposureService.Object,
                _zipFileInfoService.Object, _appSettingsConfigMock.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = MakeFakeContext().Object }
            };
        }

        private void SetupMockServices()
        {
            _logger = new Mock<ILogger<DiagnosticKeysControllerV2>>();
            _appleService = new Mock<IAppleService>(MockBehavior.Strict);
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
            _exposureKeyValidator = new Mock<IExposureKeyValidator>(MockBehavior.Strict);
            _exposureConfigurationService = new Mock<IExposureConfigurationService>(MockBehavior.Strict);
            _keyValidationConfigurationService = new Mock<IKeyValidationConfigurationService>(MockBehavior.Strict);
            _zipFileInfoService = new Mock<IZipFileInfoService>(MockBehavior.Strict);
            _addTemporaryExposureService = new Mock<IAddTemporaryExposureKeyService>(MockBehavior.Strict);
            _appSettingsConfigMock = new Mock<IAppSettingsConfig>();
            SetupMockConfiguration();
            SetupMockExposureConfigurationService();
            SetupMockZipFileInfoService();
        }

        private void SetupMockZipFileInfoService()
        {
            _zipFileInfoService.Setup(x => x.CreateZipFileInfoFromPackageName(It.IsAny<string>())).Returns(new ZipFileInfo()
            {
                BatchNumber = 1,
                PackageDate = DateTime.Today,
                Origin = "dk"
            });
            foreach (var date in InvalidPackageNames)
                _zipFileInfoService.Setup(x => x.CreateZipFileInfoFromPackageName(date)).Returns(new ZipFileInfo()
                {
                    BatchNumber = 1,
                    PackageDate = DateTime.Today.AddDays(-15),
                    Origin = "dk"
                });
            _zipFileInfoService.Setup(x => x.CheckIfPackageExists(It.IsAny<ZipFileInfo>(), It.IsAny<string>())).Returns(true);
            _zipFileInfoService.Setup(x => x.ReadPackage(It.IsAny<ZipFileInfo>(), It.IsAny<string>())).Returns(fileContent);
        }

        private void SetupMockConfiguration()
        {
            _configuration.Setup(config => config["KeyValidationRules:PackageNames:ios"]).Returns("com.netcompany.smittestop-exposure-notification");
            _configuration.Setup(config => config["KeyValidationRules:PackageNames:android"]).Returns("com.netcompany.smittestop_exposure_notification");
            _configuration.Setup(config => config["AppSettings:CacheMonitorTimeout"]).Returns("100");
            _configuration.Setup(config => config["AppSettings:fetchCommandTimeout"]).Returns("0");
            _configuration.Setup(config => config["AppSettings:enableCacheOverride"]).Returns("true");
            _configuration.Setup(config => config["ZipFilesFolder"]).Returns("");
        }

        private void SetupMockExposureConfigurationService()
        {
            _exposureConfigurationService.Setup(s => s.GetConfiguration()).Returns(new ExposureConfiguration());
        }

        private List<byte[]> MockCacheResult(int count)
        {
            List<byte[]> files = new List<byte[]>();
            for (int i = 0; i < count; i++)
            {
                files.Add(Encoding.UTF8.GetBytes("file" + i));
            }
            return files;
        }

        public static IEnumerable<string> InvalidPackageNames {
            get {
                yield return DateTime.UtcNow.AddDays(1).ToString("yyyy'-'MM'-'dd") + "_1_dk.zip";
                yield return DateTime.UtcNow.AddDays(-15).ToString("yyyy'-'MM'-'dd") + "_1_dk.zip";
                yield return "abc";
            }
        }

        public Mock<HttpContext> MakeFakeContext()
        {
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(m => m.Body)
                .Returns(new MemoryStream());

            var requestHeader = new Mock<HeaderDictionary>();
            var mockResponse = new Mock<HttpResponse>();
            var responseHeader = new Mock<HeaderDictionary>();
            mockRequest.Setup(res => res.Headers).Returns(requestHeader.Object);
            mockResponse.Setup(res => res.Headers).Returns(responseHeader.Object);
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            return mockContext;
        }

        [Test]
        public void GetExposureConfiguration_ShouldReturnOkResult()
        {
            var result = _controller.GetExposureConfiguration();
            Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void UploadDiagnosisKeys_ShouldReturnBadResultResultDueToInvalidBody()
        {
            var result = _controller.UploadDiagnosisKeys();
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        [TestCaseSource(nameof(InvalidPackageNames))]
        public void DownloadDiagnosisKeysFile_GiveInvalidDate_ShouldReturnBadRequest(string invalidDate)
        {
            var result = _controller.DownloadDiagnosisKeysFile(invalidDate);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void DownloadDiagnosisKeysFile_TodayDate_MoreFilesExist_ShouldReturnZipStream()
        {
            _zipFileInfoService.Setup(x => x.CreateZipFileInfoFromPackageName(It.IsAny<string>())).Returns(new ZipFileInfo()
            {
                BatchNumber = 1,
                PackageDate = DateTime.Today,
                Origin = "dk"
            });
            //remove Cache-Control header to make sure the test covers caching feature
            var contextWithoutCacheControl = MakeFakeContext().Object;
            _controller.ControllerContext.HttpContext = contextWithoutCacheControl;
            var result = _controller.DownloadDiagnosisKeysFile("");

            Assert.That(((FileContentResult)result.Result).FileContents, Is.Not.Empty);
            Assert.That(((FileContentResult)result.Result).ContentType, Is.EqualTo("application/zip"));
            string nextBatchExistsString = _controller.Response.Headers["nextBatchExists"].ToString();
            Assert.That(bool.Parse(nextBatchExistsString), Is.True);
        }

        [Test]
        public void ReturnBadRequest_When_RequestBodyIsNotParsable()
        {
            //Arrange
            var badRequestJsonBodyStream = new MemoryStream(Encoding.UTF8.GetBytes("attribute :**? value"));
            var contextWithoutCacheControl = MakeFakeContext();
            contextWithoutCacheControl.Setup(x => x.Request.Body).Returns(badRequestJsonBodyStream);
            contextWithoutCacheControl.Setup(x => x.Request.ContentLength).Returns(badRequestJsonBodyStream.Length);
            _controller.ControllerContext.HttpContext = contextWithoutCacheControl.Object;

            //Act
            var result = _controller.UploadDiagnosisKeys().Result;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            contextWithoutCacheControl.Verify(c => c.Request.Body, Times.Once);
            Assert.That(((BadRequestObjectResult)result).Value.ToString(), Does.StartWith("Incorrect JSON format:"));
        }

    }
}
