using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class CovidStatisticsRepositoryTests
    {
        private DigNDB_SmittestopContext _context;
        private List<CovidStatistics> _covidStatisticsMockData;
        private readonly DateTime _exampleDate = new DateTime(2020, 10, 20, 5, 5, 5);
        [SetUp]
        public void SetUp()
        {
            GenerateData();
            DbContextOptions<DigNDB_SmittestopContext> options =
                new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                    .UseInMemoryDatabase(nameof(ApplicationStatisticsRepositoryTests)).Options;
            _context = new DigNDB_SmittestopContext(options);
            _context.Database.EnsureDeleted();
            _context.AddRange(_covidStatisticsMockData);
            _context.SaveChanges();
        }

        private void GenerateData()
        {
            _covidStatisticsMockData = new List<CovidStatistics>()
            {
                new CovidStatistics()
                {
                    Id = 200,
                    Date = _exampleDate.AddDays(-1)
                },
                new CovidStatistics()
                {
                    Id = 300,
                    Date = _exampleDate.AddDays(-2)
                },
                new CovidStatistics()
                {
                    Id = 400,
                    Date = _exampleDate.AddDays(-3)
                },
            };
        }

        private CovidStatisticsRepository CreateCovidStatisticsRepository()
        {
            return new CovidStatisticsRepository(_context);
        }

        [Test]
        public void GetEntryByDate_EntryExists_EntryIsReturned()
        {
            // Arrange
            var expectedResult = new CovidStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _covidStatisticsMockData.Add(expectedResult);
            _context.Add(expectedResult);
            _context.SaveChanges();
            var covidStatisticsRepository = CreateCovidStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = covidStatisticsRepository.GetEntryByDate(
                date);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetEntryByDate_EntryDoesNotExists_NullIsReturned()
        {
            // Arrange
            var covidStatisticsRepository = CreateCovidStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = covidStatisticsRepository.GetEntryByDate(
                date);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEntryByDateAsync_EntryExists_EntryIsReturned()
        {
            // Arrange
            var expectedResult = new CovidStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _context.Add(expectedResult);
            _context.SaveChanges();
            var covidStatisticsRepository = CreateCovidStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = await covidStatisticsRepository.GetEntryByDateAsync(
                date);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetEntryByDateAsync_EntryDoesNotExists_NullIsReturned()
        {
            var covidStatisticsRepository = CreateCovidStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = await covidStatisticsRepository.GetEntryByDateAsync(
                date);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CreateEntry_EntryIsCreated()
        {
            // Arrange
            var covidStatisticsRepository = CreateCovidStatisticsRepository();
            CovidStatistics expectedResult = new CovidStatistics()
            {
                Id = 1
            };

            // Act
            covidStatisticsRepository.CreateEntry(
                expectedResult);

            // Assert
            var retrievedEntity = _context.CovidStatistics.Find(expectedResult.Id);
            Assert.AreEqual(expectedResult, retrievedEntity);
        }

        [Test]
        public async Task GetNewestEntryAsync_ShouldReturnNewestEntry()
        {
            // Arrange
            var expectedResult = new CovidStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _context.Add(expectedResult);
            _context.SaveChanges();
            var covidStatisticsRepository = CreateCovidStatisticsRepository();

            // Act
            var result = await covidStatisticsRepository.GetNewestEntryAsync();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
