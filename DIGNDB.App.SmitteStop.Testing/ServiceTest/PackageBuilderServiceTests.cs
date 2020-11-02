using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class PackageBuilderServiceTests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IDatabaseKeysToBinaryStreamMapperService> _dbToBinaryService;
        private Mock<ITemporaryExposureKeyRepository> _temporaryExposureKeyRepository;
        private Mock<IKeysListToMemoryStreamConverter> _keysListToMemoryStreamConverter;
        private DateTime _today;
        private DateTime _tomorrow;
        IPackageBuilderService _packageBuilderService;
        private List<TemporaryExposureKey> _singleKeyMockList;
        private MemoryStream _streamMock;

        [SetUp]
        public void init()
        {
            _today = DateTime.Today;
            _tomorrow = _today.AddDays(1);
            _dbToBinaryService = new Mock<IDatabaseKeysToBinaryStreamMapperService>(MockBehavior.Strict);
            _temporaryExposureKeyRepository = new Mock<ITemporaryExposureKeyRepository>(MockBehavior.Strict);
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
            _keysListToMemoryStreamConverter = new Mock<IKeysListToMemoryStreamConverter>(MockBehavior.Strict);
            SetupMocks();
            ConstructPackageBuilderService();
            _keysListToMemoryStreamConverter = new Mock<IKeysListToMemoryStreamConverter>(MockBehavior.Strict);

        }

        private void SetupMocks()
        {
            _singleKeyMockList = new List<TemporaryExposureKey>() { new TemporaryExposureKey() };
            _streamMock = new MemoryStream();
            _streamMock.Write(Encoding.Default.GetBytes("mock"));
            _dbToBinaryService.Setup(tobin => tobin.ExportDiagnosisKeys(It.IsAny<IList<TemporaryExposureKey>>()))
                .Returns(_streamMock);
            _configuration.Setup(config => config["AppSettings:MaxKeysPerFile"]).Returns("1");
            _configuration.Setup(config => config["AppSettings:FetchCommandTimeout"]).Returns("0");
            //Test1
            _temporaryExposureKeyRepository.Setup(tek => tek.GetTemporaryExposureKeysWithDkOrigin(_today, 0))
                .Returns(_singleKeyMockList);

            //Test2
            _temporaryExposureKeyRepository.Setup(tek => tek.GetTemporaryExposureKeysWithDkOrigin(_tomorrow, 0))
                .Returns(new List<TemporaryExposureKey>());
        }

        [Test]
        public void BuildPackage_GiveTodaysDate_ShouldBuildCorrectPackage()
        {
            //Arrange
            //Test1

            //Act
            var actualPackage = _packageBuilderService.BuildPackage(_today);

            //Assert
            Assert.That(actualPackage, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Not.Empty);
            Assert.That(actualPackage.FinalForTheDay, Is.EqualTo(false));
            Assert.That(actualPackage.NewerFilesExist, Is.EqualTo(false));
            Assert.That(actualPackage.CouldNotGetLock, Is.EqualTo(false));
        }

        [Test]
        public void BuildPackage_GiveTomorrowsDate_ShouldReturnNull()
        {
            //Arrange
            //Test2

            //Act
            var actualPackage = _packageBuilderService.BuildPackage(_tomorrow);

            //Assert
            Assert.That(actualPackage, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Empty);
            Assert.That(actualPackage.FinalForTheDay, Is.EqualTo(false));
            Assert.That(actualPackage.NewerFilesExist, Is.EqualTo(false));
            Assert.That(actualPackage.CouldNotGetLock, Is.EqualTo(false));
        }

        [Test]
        public void BuildPackage_GiveTodaysDate_ShouldBuildCorrectPackageDespiteNoMaxKeysPerFile()
        {
            //Arrange
            _configuration.Setup(config => config["AppSettings:MaxKeysPerFile"]).Returns("string");
            _packageBuilderService = new PackageBuilderService(_dbToBinaryService.Object, _configuration.Object,
                _temporaryExposureKeyRepository.Object, _keysListToMemoryStreamConverter.Object);            //Act
            var actualPackage = _packageBuilderService.BuildPackage(_today);

            //Assert
            Assert.That(actualPackage, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Not.Null);
            Assert.That(actualPackage.FileBytesList, Is.Not.Empty);
            Assert.That(actualPackage.FinalForTheDay, Is.EqualTo(false));
            Assert.That(actualPackage.NewerFilesExist, Is.EqualTo(false));
            Assert.That(actualPackage.CouldNotGetLock, Is.EqualTo(false));
        }

        [TestCase("0", ExpectedResult = 0)]
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("1234", ExpectedResult = 1234)]
        [TestCase("Does not parse", ExpectedResult = 750000)]
        [TestCase("1234.56", ExpectedResult = 750000)]
        public int MaxKeysPerFile_IsParsed_Correctly(string value)
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["AppSettings:MaxKeysPerFile"]).Returns(value);

            // Act
            var sut = new PackageBuilderService(null, configurationMock.Object, null, _keysListToMemoryStreamConverter.Object);

            // Assert
            return sut.MaxKeysPerFile;
        }

        [Test]
        public void BuildPackageContentV2TestShouldReturnCorrectPackageContentForAllOrigin()
        {
            var startDate = DateTime.UtcNow;
            var firstMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey(), new TemporaryExposureKey() };
            var secondMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey() };
            var firstPackageContent = new byte[] { 1, 2, 3, 4 };
            var secondPackageContent = new byte[] { 1, 2, 3, 4 };
            _configuration.Setup(config => config["AppSettings:MaxKeysPerFile"]).Returns("2");
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 0, 2)).Returns(firstMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 2, 2)).Returns(secondMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 3, 2)).Returns(new List<TemporaryExposureKey>());
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(firstMockedKeyPackage)).Returns(firstPackageContent);
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(secondMockedKeyPackage)).Returns(secondPackageContent);
            ConstructPackageBuilderService();
            var result = _packageBuilderService.BuildPackageContentV2(startDate, ZipFileOrigin.All);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0], firstPackageContent);
            Assert.AreEqual(result[1], secondPackageContent);
        }

        [Test]
        public void BuildPackageContentV2TestShouldReturnCorrectPackageContentForDkOrigin()
        {
            var startDate = DateTime.UtcNow;
            var firstMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey(), new TemporaryExposureKey() };
            var secondMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey() };
            var firstPackageContent = new byte[] { 1, 2, 3, 4 };
            var secondPackageContent = new byte[] { 1, 2, 3, 4 };
            _configuration.Setup(config => config["AppSettings:MaxKeysPerFile"]).Returns("2");
            _temporaryExposureKeyRepository.Setup(x => x.GetDkTemporaryExposureKeysForPeriodNextBatch(startDate, 0, 2)).Returns(firstMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetDkTemporaryExposureKeysForPeriodNextBatch(startDate, 2, 2)).Returns(secondMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetDkTemporaryExposureKeysForPeriodNextBatch(startDate, 3, 2)).Returns(new List<TemporaryExposureKey>());
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(firstMockedKeyPackage)).Returns(firstPackageContent);
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(secondMockedKeyPackage)).Returns(secondPackageContent);
            ConstructPackageBuilderService();
            var result = _packageBuilderService.BuildPackageContentV2(startDate, ZipFileOrigin.Dk);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0], firstPackageContent);
            Assert.AreEqual(result[1], secondPackageContent);
        }

        private void ConstructPackageBuilderService()
        {
            _packageBuilderService = new PackageBuilderService(_dbToBinaryService.Object, _configuration.Object,
        _temporaryExposureKeyRepository.Object, _keysListToMemoryStreamConverter.Object);
        }
    }
}

