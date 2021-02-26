using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class SSIStatisticsRepositoryTests
    {
        private DigNDB_SmittestopContext _context;
        private List<SSIStatistics> _ssiStatisticsMockData;
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
            _context.AddRange(_ssiStatisticsMockData);
            _context.SaveChanges();
        }

        private void GenerateData()
        {
            _ssiStatisticsMockData = new List<SSIStatistics>()
            {
                new SSIStatistics()
                {
                    Id = 200,
                    Date = _exampleDate.AddDays(-1)
                },
                new SSIStatistics()
                {
                    Id = 300,
                    Date = _exampleDate.AddDays(-2)
                },
                new SSIStatistics()
                {
                    Id = 400,
                    Date = _exampleDate.AddDays(-3)
                },
            };
        }

        private SSIStatisticsRepository CreateSSIStatisticsRepository()
        {
            return new SSIStatisticsRepository(_context);
        }

        [Test]
        public void GetEntryByDate_EntryExists_EntryIsReturned()
        {
            // Arrange
            var expectedResult = new SSIStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _ssiStatisticsMockData.Add(expectedResult);
            _context.Add(expectedResult);
            _context.SaveChanges();
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = sSIStatisticsRepository.GetEntryByDate(
                date);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetEntryByDate_EntryDoesNotExists_NullIsReturned()
        {
            // Arrange
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = sSIStatisticsRepository.GetEntryByDate(
                date);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEntryByDateAsync_EntryExists_EntryIsReturned()
        {
            // Arrange
            var expectedResult = new SSIStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _context.Add(expectedResult);
            _context.SaveChanges();
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = await sSIStatisticsRepository.GetEntryByDateAsync(
                date);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetEntryByDateAsync_EntryDoesNotExists_NullIsReturned()
        {
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            var result = await sSIStatisticsRepository.GetEntryByDateAsync(
                date);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CreateEntry_EntryIsCreated()
        {
            // Arrange
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            SSIStatistics expectedResult = new SSIStatistics()
            {
                Id = 1
            };

            // Act
            sSIStatisticsRepository.CreateEntry(
                expectedResult);

            // Assert
            var retrievedEntity = _context.SSIStatistics.Find(expectedResult.Id);
            Assert.AreEqual(expectedResult, retrievedEntity);
        }

        [Test]
        public void RemoveEntriesOlderThan_ShouldRemoveAllOlderRecords()
        {
            // Arrange
            var expectedResult = new SSIStatistics()
            {
                Id = 5,
                Date = _exampleDate.AddDays(1)
            };
            _context.Add(expectedResult);
            _context.SaveChanges();
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();
            DateTime date = _exampleDate;

            // Act
            sSIStatisticsRepository.RemoveEntriesOlderThan(
                date);

            // Assert
            Assert.AreEqual(1, _context.SSIStatistics.Count());
            var retrievedEntity = _context.SSIStatistics.Find(expectedResult.Id);
            Assert.AreEqual(expectedResult, retrievedEntity);
        }

        [Test]
        public async Task GetNewestEntryAsync_ShouldReturnNewestEntry()
        {
            // Arrange
            var expectedResult = new SSIStatistics()
            {
                Id = 5,
                Date = _exampleDate
            };
            _context.Add(expectedResult);
            _context.SaveChanges();
            var sSIStatisticsRepository = CreateSSIStatisticsRepository();

            // Act
            var result = await sSIStatisticsRepository.GetNewestEntryAsync();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
