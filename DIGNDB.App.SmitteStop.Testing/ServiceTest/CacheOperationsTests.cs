using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;
using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class CacheOperationsTests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IPackageBuilderService> _cachePackageBuilder;
        CacheOperations _cacheService;
        MemoryCache _memoryCache;
        private DateTime dateAsKey;

        [SetUp]
        public void init()
        {
            SetupMockConfiguration();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            dateAsKey = DateTime.UtcNow.AddDays(-1).Date;
            _memoryCache.Set(dateAsKey, mockCacheResult);
            _cacheService = new CacheOperations(_memoryCache, _configuration.Object, _cachePackageBuilder.Object);
        }

        private CacheResult mockCacheResult =>
            new CacheResult() { FileBytesList = new List<byte[]> { Encoding.UTF8.GetBytes("mocked") } };


        private void SetupMockConfiguration()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(config => config["AppSettings:CacheMonitorTimeout"]).Returns("100");
            _configuration.Setup(config => config["AppSettings:PreviousDayFileCaching"]).Returns("15.00:00:00.000");
            _configuration.Setup(config => config["AppSettings:CurrentDayFileCaching"]).Returns("02:00:00.000");
            _cachePackageBuilder = new Mock<IPackageBuilderService>();
        }

        [Test]
        public void GetCacheValue_GiveData_ShouldCacheDataAndReturnCorrectCacheResult()
        {
            var expectCacheResult = mockCacheResult;
            var actualCacheResult = _cacheService.GetCacheValue(dateAsKey).Result;

            Assert.IsNotNull(_memoryCache.Get(dateAsKey));
            Assert.IsNotNull(actualCacheResult);
            Assert.IsNotNull(actualCacheResult.FileBytesList);
            CollectionAssert.AreEqual(expectCacheResult.FileBytesList, actualCacheResult.FileBytesList);
        }

    }
}
