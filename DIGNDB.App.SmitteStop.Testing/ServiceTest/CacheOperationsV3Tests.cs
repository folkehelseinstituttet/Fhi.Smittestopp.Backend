using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class CacheOperationsV2Tests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IZipFileInfoService> _zipFileInfoService;
        private Mock<ILogger<CacheOperationsV3>> _logger;
        private CacheOperationsV3 _cacheService;
        private MemoryCache _memoryCache;
        private ZipFileInfo _zipFileInfo;

        [SetUp]
        public void Init()
        {
            SetupMockConfiguration();
            _zipFileInfoService = new Mock<IZipFileInfoService>(MockBehavior.Strict);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<CacheOperationsV3>>();
            _zipFileInfo = new ZipFileInfo()
            {
                BatchNumber = 1,
                Origin = "dk",
                PackageDate = DateTime.UtcNow.AddDays(-1).Date
            };
            _memoryCache.Set(_zipFileInfo.FileName, mockCacheResult);
            _cacheService = new CacheOperationsV3(_memoryCache, _configuration.Object, _logger.Object, _zipFileInfoService.Object);
        }

        private byte[] mockCacheResult => Encoding.UTF8.GetBytes("mocked");


        private void SetupMockConfiguration()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(config => config["AppSettings:CacheMonitorTimeout"]).Returns("100");
            _configuration.Setup(config => config["AppSettings:PreviousDayFileCaching"]).Returns("15.00:00:00.000");
        }

        [Test]
        public void GetCacheValue_GiveData_ShouldCacheDataAndReturnCorrectCacheResult()
        {
            var expectCacheResult = mockCacheResult;
            var actualCacheResult = _cacheService.GetCacheValue(_zipFileInfo, "mockedFolder").Result;

            Assert.IsNotNull(_memoryCache.Get(_zipFileInfo.FileName));
            Assert.IsNotNull(actualCacheResult);
            CollectionAssert.AreEqual(expectCacheResult, actualCacheResult);
        }

    }
}
