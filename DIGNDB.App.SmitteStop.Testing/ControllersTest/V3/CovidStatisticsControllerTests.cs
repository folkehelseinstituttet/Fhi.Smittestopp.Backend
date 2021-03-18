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
        private Mock<ICovidStatisticsRepository> _mockCovidStatisticsRepository;
        private Mock<IMapper> _mockMapper;

        private ApplicationStatistics _appStatisticsEntry;
        private readonly DateTime _covidStatisticsPackageDate = new DateTime(2020, 10, 20, 5, 5, 3);
        private readonly DateTime _appStatisticsPackageDate = new DateTime(2020, 10, 10, 5, 5, 3);
        private CovidStatistics _covidStatisticsEntry;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockLogger = _mockRepository.Create<ILogger<CovidStatisticsControllerV3>>(MockBehavior.Loose);
            _mockApplicationStatisticsRepository = _mockRepository.Create<IApplicationStatisticsRepository>();
            _mockCovidStatisticsRepository = _mockRepository.Create<ICovidStatisticsRepository>();
            _mockMapper = _mockRepository.Create<IMapper>(MockBehavior.Loose);
            _appStatisticsEntry = new ApplicationStatistics()
            {
                EntryDate = _appStatisticsPackageDate,
                Id = 1,
                PositiveResultsLast7Days = 1000,
                PositiveTestsResultsTotal = 2000,
                SmittestopDownloadsTotal = 3000
            };
            _covidStatisticsEntry = new CovidStatistics()
            {
                ConfirmedCasesTotal = 100,
                ConfirmedCasesToday = 200,
                EntryDate = _covidStatisticsPackageDate,
                Id = 1,
                PatientsAdmittedToday = 500,
                TestsConductedToday = 600,
                TestsConductedTotal = 700,
                VaccinatedFirstDoseToday = 800,
                VaccinatedSecondDoseToday = 900,
                VaccinatedFirstDoseTotal = 1000,
                VaccinatedSecondDoseTotal = 1100
            };
        }

        private CovidStatisticsControllerV3 CreateCovidStatisticsController()
        {
            return new CovidStatisticsControllerV3(
                _mockLogger.Object,
                _mockApplicationStatisticsRepository.Object,
                _mockCovidStatisticsRepository.Object,
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
            _mockCovidStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(value: null);
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
            string packageDate = _covidStatisticsPackageDate.Date.ToString();
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockCovidStatisticsRepository.Setup(x => x.GetEntryByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_covidStatisticsEntry);
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
            _mockCovidStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_covidStatisticsEntry);
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
            string packageDate = _covidStatisticsPackageDate.Date.ToString();
            _mockApplicationStatisticsRepository.Setup(x => x.GetNewestEntryAsync()).ReturnsAsync(_appStatisticsEntry);
            _mockCovidStatisticsRepository.Setup(x => x.GetEntryByDateAsync(It.IsAny<DateTime>()))
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
