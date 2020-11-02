using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using Moq;
using NUnit.Framework;
using System;
using System.IO;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ZipFileInfoServiceTests
    {
        private Mock<IFileSystem> _fileSystem;
        private IZipFileInfoService _fileInfoService;

        [SetUp]
        public void Init()
        {
            _fileSystem = new FileSystemMockFactory().GetMock();
            _fileInfoService = new ZipFileInfoService(_fileSystem.Object);
        }

        [Test]
        public void CreateZipFileInfoFromPackageNameTestShouldReturnCorrectPackageName()
        {
            string filename = "2020-09-22_2_dk.zip";
            var actualZipFileInfo = _fileInfoService.CreateZipFileInfoFromPackageName(filename);
            var expectedZipFileInfo = new ZipFileInfo()
            {
                BatchNumber = 2,
                PackageDate = DateTime.Parse("2020-09-22"),
                Origin = "dk"
            };
            Assert.AreEqual(expectedZipFileInfo.Origin, actualZipFileInfo.Origin);
            Assert.AreEqual(expectedZipFileInfo.BatchNumber, actualZipFileInfo.BatchNumber);
            Assert.AreEqual(expectedZipFileInfo.PackageDate, actualZipFileInfo.PackageDate);
        }

        [TestCase("2020-09-24", 1)]
        [TestCase("2020-09-22", 5)]
        public void GetNextBatchNumberForGivenDayTestShouldReturnCorrectBatchNumber(string value, int expectedBatchNumber)
        {
            var date = DateTime.Parse(value);
            var paths = new string[] { "2020-09-22_2_dk.zip", "2020-09-22_3_dk.zip", "2020-09-22_4_dk.zip", "2020-09-23_2_dk.zip" };
            int actualBatchNumber = _fileInfoService.GetNextBatchNumberForGivenDay(paths, date);
            Assert.AreEqual(expectedBatchNumber, actualBatchNumber);
        }

        [Test]
        public void CheckIfPackageExistsTestShouldCheckForRightFile()
        {
            var date = DateTime.UtcNow;
            ZipFileInfo zipFileInfo = new ZipFileInfo()
            {
                BatchNumber = 2,
                Origin = "dk",
                PackageDate = date
            };
            string zipFilesFolder = "ZipFilesFolder";
            _fileSystem.Setup(x => x.FileExists(Path.Join(zipFilesFolder, zipFileInfo.Origin.ToLower(), zipFileInfo.FileName))).Returns(true);
            bool actualValue = _fileInfoService.CheckIfPackageExists(zipFileInfo, zipFilesFolder);
            Assert.IsTrue(actualValue);
        }

        [Test]
        public void ReadPackageTestShouldReadRightFile()
        {
            var date = DateTime.UtcNow;
            ZipFileInfo zipFileInfo = new ZipFileInfo()
            {
                BatchNumber = 2,
                Origin = "dk",
                PackageDate = date
            };
            string zipFilesFolder = "ZipFilesFolder";
            byte[] expectedContent = new byte[] { 1, 2, 3, 4 };
            _fileSystem.Setup(x => x.ReadFile(Path.Join(zipFilesFolder, zipFileInfo.Origin.ToLower(), zipFileInfo.FileName))).Returns(expectedContent);
            var actualContent = _fileInfoService.ReadPackage(zipFileInfo, zipFilesFolder);
            Assert.AreEqual(expectedContent, actualContent);
        }
    }
}
