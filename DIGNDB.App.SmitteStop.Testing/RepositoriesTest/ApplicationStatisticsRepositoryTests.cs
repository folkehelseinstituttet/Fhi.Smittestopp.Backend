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
    public class ApplicationStatisticsRepositoryTests
    {

        private DigNDB_SmittestopContext _context;
        private List<ApplicationStatistics> _appStatisticsMockData;

        [SetUp]
        public void SetUp()
        {
            GenerateData();
            DbContextOptions<DigNDB_SmittestopContext> options =
                new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                    .UseInMemoryDatabase(nameof(ApplicationStatisticsRepositoryTests)).Options;
            _context = new DigNDB_SmittestopContext(options);
        }

        private void GenerateData()
        {
            _appStatisticsMockData = new List<ApplicationStatistics>()
            {
                new ApplicationStatistics()
                {
                    EntryDate = new DateTime(2020, 10, 20),
                    Id = 1,
                    PositiveResultsLast7Days = 100,
                    PositiveTestsResultsTotal = 200,
                    SmittestopDownloadsTotal = 300
                },
                new ApplicationStatistics()
                {
                    EntryDate = new DateTime(2020, 10, 21),
                    Id = 2,
                    PositiveResultsLast7Days = 1100,
                    PositiveTestsResultsTotal = 2100,
                    SmittestopDownloadsTotal = 3100
                }
            };
        }

        private ApplicationStatisticsRepository CreateApplicationStatisticsRepository()
        {
            return new ApplicationStatisticsRepository(_context);
        }

        [Test]
        public async Task GetNewestEntryAsync_EntriesExist_ShouldReturnNewestEntry()
        {
            // Arrange
            _context.AddRange(_appStatisticsMockData);
            _context.SaveChanges();
            var applicationStatisticsRepository = CreateApplicationStatisticsRepository();

            // Act
            var result = await applicationStatisticsRepository.GetNewestEntryAsync();

            // Assert
            Assert.AreEqual(_appStatisticsMockData[1], result);
        }

        [Test]
        public async Task GetNewestEntryAsync_EntriesDoNotExist_ShouldReturnNull()
        {
            // Arrange
            var applicationStatisticsRepository = CreateApplicationStatisticsRepository();

            // Act
            var result = await applicationStatisticsRepository.GetNewestEntryAsync();

            // Assert
            Assert.IsNull(result);
        }
    }
}
