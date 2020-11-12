using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class PackageBuilderServiceTests
    {

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
            _keysListToMemoryStreamConverter = new Mock<IKeysListToMemoryStreamConverter>(MockBehavior.Strict);
            SetupMocks();
            ConstructPackageBuilderService(new AppSettingsConfig() { MaxKeysPerFile = 1, FetchCommandTimeout = 0});
            _keysListToMemoryStreamConverter = new Mock<IKeysListToMemoryStreamConverter>(MockBehavior.Strict);

        }

        private void SetupMocks()
        {
            _singleKeyMockList = new List<TemporaryExposureKey>() { new TemporaryExposureKey() };
            _streamMock = new MemoryStream();
            _streamMock.Write(Encoding.Default.GetBytes("mock"));
            _dbToBinaryService.Setup(tobin => tobin.ExportDiagnosisKeys(It.IsAny<IList<TemporaryExposureKey>>()))
                .Returns(_streamMock);
            //Test1
            _temporaryExposureKeyRepository.Setup(tek => tek.GetKeysOnlyFromApiOriginCountry(_today, 0))
                .Returns(_singleKeyMockList);

            //Test2
            _temporaryExposureKeyRepository.Setup(tek => tek.GetKeysOnlyFromApiOriginCountry(_tomorrow, 0))
                .Returns(new List<TemporaryExposureKey>());
        }

        [Test]
        public void BuildPackage_GiveTodaysDate_ShouldBuildCorrectPackage()
        {
            //Arrange
            //Test1
            ConstructPackageBuilderService(new AppSettingsConfig() { MaxKeysPerFile = 2 });
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
            ConstructPackageBuilderService(new AppSettingsConfig() { MaxKeysPerFile = 2 });
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
        public void BuildPackageContentV2TestShouldReturnCorrectPackageContentForAllOrigin()
        {
            var startDate = DateTime.UtcNow;
            var firstMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey(), new TemporaryExposureKey() };
            var secondMockedKeyPackage = new List<TemporaryExposureKey>() { new TemporaryExposureKey() };
            var firstPackageContent = new byte[] { 1, 2, 3, 4 };
            var secondPackageContent = new byte[] { 1, 2, 3, 4 };
            var maxKeysPerFile = 2;
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 0, 2)).Returns(firstMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 2, 2)).Returns(secondMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetAllTemporaryExposureKeysForPeriodNextBatch(startDate, 3, 2)).Returns(new List<TemporaryExposureKey>());
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(firstMockedKeyPackage)).Returns(firstPackageContent);
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(secondMockedKeyPackage)).Returns(secondPackageContent);

            ConstructPackageBuilderService(new AppSettingsConfig() { MaxKeysPerFile = maxKeysPerFile });

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
            var maxKeysPerFile = 2;
            _temporaryExposureKeyRepository.Setup(x => x.GetOriginCountryKeysForPeriodNextBatch(startDate, 0, 2)).Returns(firstMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetOriginCountryKeysForPeriodNextBatch(startDate, 2, 2)).Returns(secondMockedKeyPackage);
            _temporaryExposureKeyRepository.Setup(x => x.GetOriginCountryKeysForPeriodNextBatch(startDate, 3, 2)).Returns(new List<TemporaryExposureKey>());
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(firstMockedKeyPackage)).Returns(firstPackageContent);
            _keysListToMemoryStreamConverter.Setup(x => x.ConvertKeysToMemoryStream(secondMockedKeyPackage)).Returns(secondPackageContent);

            ConstructPackageBuilderService( new AppSettingsConfig() { MaxKeysPerFile = maxKeysPerFile } );

            var result = _packageBuilderService.BuildPackageContentV2(startDate, ZipFileOrigin.Dk);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0], firstPackageContent);
            Assert.AreEqual(result[1], secondPackageContent);
        }

        private void ConstructPackageBuilderService(IPackageBuilderConfig configuration)
        {
            _packageBuilderService = new PackageBuilderService(
                _dbToBinaryService.Object,
                configuration,
                _temporaryExposureKeyRepository.Object,
                _keysListToMemoryStreamConverter.Object
            );
        }
    }
}

