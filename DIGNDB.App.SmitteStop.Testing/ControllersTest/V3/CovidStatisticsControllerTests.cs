using AutoMapper;
using DIGNDB.App.SmitteStop.API.V3.Controllers;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ControllersTest.V2
{
    [TestFixture]
    public class CovidStatisticsControllerTests
    {
        private MockRepository _mockRepository;

        private Mock<ILogger<CovidStatisticsControllerV3>> _mockLogger;
        private Mock<IApplicationStatisticsRepository> _mockApplicationStatisticsRepository;
        private Mock<ISSIStatisticsRepository> _mockSSIStatisticsRepository;
        private Mock<IMapper> _mockMapper;

        private ApplicationStatistics _appStatisticsEntry;
        private readonly DateTime _ssiPackageDate = new DateTime(2020, 10, 20, 5, 5, 3);
        private readonly DateTime _appPackageDate = new DateTime(2020, 10, 10, 5, 5, 3);
        private SSIStatistics _ssiStatisticsEntry;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockLogger = _mockRepository.Create<ILogger<CovidStatisticsControllerV3>>(MockBehavior.Loose);
            _mockApplicationStatisticsRepository = _mockRepository.Create<IApplicationStatisticsRepository>();
            _mockSSIStatisticsRepository = _mockRepository.Create<ISSIStatisticsRepository>();
            _mockMapper = _mockRepository.Create<IMapper>(MockBehavior.Loose);
            _appStatisticsEntry = new ApplicationStatistics()
            {
                EntryDate = _appPackageDate,
                Id = 1,
                PositiveResultsLast7Days = 1000,
                PositiveTestsResultsTotal = 2000,
                TotalSmittestopDownloads = 3000
            };
            _ssiStatisticsEntry = new SSIStatistics()
            {
                ConfirmedCasesTotal = 100,
                ConfirmedCasesToday = 200,
                Date = _ssiPackageDate,
                Id = 1,
                PatientsAdmittedToday = 500,
                TestsConductedToday = 600,
                TestsConductedTotal = 700,
                VaccinatedFirstDose = 0.1,
                VaccinatedSecondDose = 0.05
            };
        }

        private CovidStatisticsControllerV3 CreateCovidStatisticsController()
        {
            return new CovidStatisticsControllerV3(
                _mockLogger.Object,
                _mockApplicationStatisticsRepository.Object,
                _mockSSIStatisticsRepository.Object,
                _mockMapper.Object);
        }

        [Test]
        public async Task GetCovidStatistics_NoApplicationStatistics_ShouldReturnBadRequest()
        {
            // Arrange
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(value: null);
            var covidStatisticsController = CreateCovidStatisticsController();
            string packageDate = null;

            // Act
            var result = await covidStatisticsController.GetCovidStatistics(
                packageDate);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public async Task GetCovidStatistics_FetchingNewestPackage_NoPackagesExist_ShouldReturnNoContent()
        {
            // Arrange
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockSSIStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(value: null);
            var covidStatisticsController = CreateCovidStatisticsController();
            string packageDate = null;

            // Act
            var result = await covidStatisticsController.GetCovidStatistics(
                packageDate);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }

        [Test]
        public async Task GetCovidStatistics_FetchingSpecificPackage_PackagesExist_ShouldReturnPackage()
        {
            // Arrange
            string packageDate = _ssiPackageDate.Date.ToString();
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockSSIStatisticsRepository.Setup(x => x.GetEntryByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_ssiStatisticsEntry);
            var covidStatisticsController = CreateCovidStatisticsController();

            // Act
            var result = await covidStatisticsController.GetCovidStatistics(
                packageDate);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
        }

        [Test]
        public async Task GetCovidStatistics_FetchingNewestPackage_PackagesExists_ShouldReturnPackage()
        {
            // Arrange
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockSSIStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_ssiStatisticsEntry);
            var covidStatisticsController = CreateCovidStatisticsController();
            string packageDate = null;

            // Act
            var result = await covidStatisticsController.GetCovidStatistics(
                packageDate);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
        }

        [Test]
        public async Task GetCovidStatistics_FetchingSpecificPackage_NoPackagesExists_ShouldReturnNoContent()
        {
            // Arrange
            string packageDate = _ssiPackageDate.Date.ToString();
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockSSIStatisticsRepository.Setup(x => x.GetEntryByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(value: null);
            var covidStatisticsController = CreateCovidStatisticsController();

            // Act
            var result = await covidStatisticsController.GetCovidStatistics(
                packageDate);

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }
    }
}
