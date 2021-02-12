using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class TemporaryExposureKeyRepositoryTests
    {
        private TemporaryExposureKeyRepository _repo;
        private DbContextOptions<DigNDB_SmittestopContext> _options;
        private Mock<ICountryRepository> _countryRepository;
        private Mock<ILogger<TemporaryExposureKeyRepository>> _logger;

        private readonly Country _dkCountry = new Country()
        {
            Id = 1,
            Code = "dk"
        };

        private readonly Country _notDkCountry = new Country()
        {
            Id = 2,
            Code = "not dk"
        };

        [SetUp]
        public void Init()
        {
            _logger = new Mock<ILogger<TemporaryExposureKeyRepository>>(MockBehavior.Loose);
        }

        [SetUp]
        public void CreateOptions()
        {
            var DBName = "TEST_DB_" + DateTime.UtcNow;
            _options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>().UseInMemoryDatabase(databaseName: DBName).Options;
            _countryRepository = new Mock<ICountryRepository>(MockBehavior.Strict);
            _countryRepository.Setup(x => x.GetApiOriginCountry()).Returns(_dkCountry);
        }

        private IList<TemporaryExposureKey> CreateMockedListExposureKeys(DateTime expectDate, int numberOfKeys, bool isDkOrigin)
        {
            List<TemporaryExposureKey> data = new List<TemporaryExposureKey>();
            for (int i = 0; i < numberOfKeys; i++)
            {
                data.Add(new TemporaryExposureKey()
                {
                    CreatedOn = expectDate.Date,
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData" + (i + 1)),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW,
                    Origin = _dkCountry
                });
                if (!isDkOrigin)
                    data.Last().Origin = _notDkCountry;
            }
            return data;
        }

        [Test]
        public void GetTemporaryExposureKeys_HaveRecord_ShouldReturnCorrectRecordMatchedRequirement()
        {
            var expectDate = DateTime.UtcNow;
            var dataForCurrentDate = CreateMockedListExposureKeys(expectDate, 2, true);
            var dataForOtherDate = CreateMockedListExposureKeys(expectDate.AddDays(-12), 3, true);
            var dataForNotDK = CreateMockedListExposureKeys(expectDate.AddDays(-12), 3, false);
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                //add data to context
                context.TemporaryExposureKey.AddRange(dataForCurrentDate);
                context.TemporaryExposureKey.AddRange(dataForOtherDate);
                context.TemporaryExposureKey.AddRange(dataForNotDK);
                context.SaveChanges();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                var keys = _repo.GetKeysOnlyFromApiOriginCountry(expectDate, 0);
                Assert.AreEqual(dataForCurrentDate.Count, keys.Count);
            }
        }

        [Test]
        public void GetById_HaveRecord_ShouldReturnCorrectRecord()
        {
            var data = CreateMockedListExposureKeys(DateTime.UtcNow, 4, true);
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                context.TemporaryExposureKey.AddRange(data);
                context.SaveChanges();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                var expectKey = data[0];
                var actualKey = _repo.GetById(expectKey.Id).Result;

                Assert.AreEqual(expectKey.Id, actualKey.Id);
                Assert.AreEqual(expectKey.TransmissionRiskLevel, actualKey.TransmissionRiskLevel);
                Assert.AreEqual(expectKey.CreatedOn, actualKey.CreatedOn);
                Assert.AreEqual(expectKey.KeyData, actualKey.KeyData);
            }
        }

        [Test]
        public void GetAll_HaveData_ShouldReturnCorrectNumberOfRecord()
        {
            var expectKeys = 4;
            var data = CreateMockedListExposureKeys(DateTime.UtcNow, expectKeys, true);
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                //add data to context
                context.TemporaryExposureKey.AddRange(data);
                context.SaveChanges();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                var keys = _repo.GetAll().Result;
                Assert.AreEqual(expectKeys, keys.Count);
            }
        }

        [Test]
        public void GetAllKeyData_HaveData_ShouldReturnCorrectResult()
        {
            var data = CreateMockedListExposureKeys(DateTime.UtcNow, 4, true);
            var expectKeysData = data.Select(x => x.KeyData).ToList();
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                //add data to context
                context.TemporaryExposureKey.AddRange(data);
                context.SaveChanges();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                var keys = _repo.GetAllKeyData().Result;
                CollectionAssert.AreEqual(expectKeysData, keys);
            }
        }

        [Test]
        public void AddTemporaryExposureKey_ProvideKey_ShouldAddNewKeyToDB()
        {
            TemporaryExposureKey key = new TemporaryExposureKey()
            {
                CreatedOn = DateTime.UtcNow.Date,
                Id = Guid.NewGuid(),
                KeyData = Encoding.ASCII.GetBytes("keyData"),
                TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW,
            };
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                _repo.AddTemporaryExposureKey(key).Wait();
            }
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                var keyInDB = context.TemporaryExposureKey.ToList();
                Assert.AreEqual(1, keyInDB.Count);
                Assert.IsNotNull(keyInDB.FirstOrDefault(k => k.Id == key.Id));
            }
        }

        // This test needs to be fixed however I don't know how to do that :(
        [Test]
        public void AddTemporaryExposureKeys_ProvideKeys_ShouldAddNewKeysToDB()
        {
            var data = CreateMockedListExposureKeys(DateTime.UtcNow, 4, false);
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                context.Database.EnsureDeleted();
                _repo = new TemporaryExposureKeyRepository(context, _countryRepository.Object, _logger.Object);
                _repo.AddTemporaryExposureKeysAsync(data).Wait();
            }
            using (var context = new DigNDB_SmittestopContext(_options))
            {
                var keyInDB = context.TemporaryExposureKey.ToList();
                Assert.AreEqual(4, keyInDB.Count);
            }
        }
    }
}
