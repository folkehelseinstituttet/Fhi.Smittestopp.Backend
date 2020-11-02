using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ZipFileServiceTests
    {
        private IZipFileService _zipFileService;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IConfiguration> _configuration;
        private Mock<IZipFileInfoService> _zipFileInfoService;
        private Mock<IPackageBuilderService> _packageBuilder;
        private string _zipFilesFolder = "ZipsFolder";
        private string _dkZipFilesFolder;
        private string _allZipFilesFolder;

        [SetUp]
        public void Init()
        {
            _fileSystem = new FileSystemMockFactory().GetMock();
            _configuration = new ConfigurationMockFactory().GetMock();
            _zipFileInfoService = new ZipFileInfoServiceMockFactory().GetMock();
            _packageBuilder = new PackageBuilderMockFactory().GetMock();
            _configuration.Setup(x => x["ZipFilesFolder"]).Returns(_zipFilesFolder);
            _dkZipFilesFolder = Path.Join(_zipFilesFolder, _dkZipFilesFolder);
            _allZipFilesFolder = Path.Join(_zipFilesFolder, _allZipFilesFolder);
        }

        [Test]
        public void UpdateZipFilesTestShouldCreateNewDirectories()
        {
            _zipFileService = new ZipFileService(_configuration.Object, _packageBuilder.Object, _zipFileInfoService.Object, _fileSystem.Object);
            _zipFileService.UpdateZipFiles(DateTime.Now.AddDays(-1), DateTime.Now);
            _fileSystem.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test]
        public void UpdateZipFilesTestShouldCreateFourBatches()
        {
            _zipFileService = new ZipFileService(_configuration.Object, _packageBuilder.Object, _zipFileInfoService.Object, _fileSystem.Object);
            _zipFileService.UpdateZipFiles(DateTime.Now.AddDays(-1), DateTime.Now);
            _fileSystem.Verify(x => x.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Exactly(4));
        }

        [TestCase("dk")]
        [TestCase("all")]
        public void UpdateZipFilesTestShouldCreateTwoBatches(string value)
        {
            if (value == "dk")
            {
                _packageBuilder.Setup(x => x.BuildPackageContentV2(It.IsAny<DateTime>(), ZipFileOrigin.Dk)).Returns(new List<byte[]>());
            }
            if (value == "all")
            {
                _packageBuilder.Setup(x => x.BuildPackageContentV2(It.IsAny<DateTime>(), ZipFileOrigin.All)).Returns(new List<byte[]>());
            }
            _zipFileService = new ZipFileService(_configuration.Object, _packageBuilder.Object, _zipFileInfoService.Object, _fileSystem.Object);
            _zipFileService.UpdateZipFiles(DateTime.Now.AddDays(-1), DateTime.Now);
            _fileSystem.Verify(x => x.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Exactly(2));
        }

        [Test]
        public void UpdateZipFilesTestShouldNotCreateBatches()
        {
            _packageBuilder.Setup(x => x.BuildPackageContentV2(It.IsAny<DateTime>(), It.IsAny<ZipFileOrigin>())).Returns(new List<byte[]>());
            _zipFileService = new ZipFileService(_configuration.Object, _packageBuilder.Object, _zipFileInfoService.Object, _fileSystem.Object);
            _zipFileService.UpdateZipFiles(DateTime.Now.AddDays(-1), DateTime.Now);
            _fileSystem.Verify(x => x.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
        }
    }
}
