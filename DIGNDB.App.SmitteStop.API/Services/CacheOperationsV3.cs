using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CacheOperationsV3 : ICacheOperationsV3
    {
        private const int DefaultCacheMonitorTimeout = 100;
        private readonly ILogger<CacheOperationsV3> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly IZipFileInfoService _zipFileInfoService;

        private readonly TimeSpan _fileCachingTimespan;
        public static readonly object _cacheLock = new object();
        private bool _lockTaken;
        private TimeSpan _timeout;

        public CacheOperationsV3(IMemoryCache memoryCache, IConfiguration configuration, ILogger<CacheOperationsV3> logger, IZipFileInfoService zipFileInfoService)
        {
            _zipFileInfoService = zipFileInfoService;
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _fileCachingTimespan = TimeSpan.Parse(_configuration["AppSettings:PreviousDayFileCaching"]);
            _lockTaken = false;
        }

        public async Task<byte[]> GetCacheValue(ZipFileInfo zipFileInfo, string zipFilesFolder, bool forceRefresh = false)
        {
            return TryGetPackageCreateIfNotExisting(zipFileInfo, zipFilesFolder, forceRefresh);
        }

        private byte[] TryGetPackageCreateIfNotExisting(ZipFileInfo zipFileInfo, string zipFilesFolder, bool forceRefresh)
        {
            if (!_memoryCache.TryGetValue(zipFileInfo.FileName, out byte[] result))
            {
                RetrieveTimeoutFromConfig();

                result = TryCreateNewCachedPackage(zipFileInfo, zipFilesFolder, forceRefresh);
            }

            return result;
        }

        private byte[] TryCreateNewCachedPackage(ZipFileInfo zipFileInfo, string zipFilesFolder, bool forceRefresh)
        {
            try
            {
                Monitor.TryEnter(_cacheLock, _timeout, ref _lockTaken);
                if (_lockTaken)
                {
                    return CreateNewPackageOnlyIfNotCreatedEarlier(zipFileInfo, zipFilesFolder, forceRefresh);
                }
                else
                {
                    throw new SynchronizationLockException();
                }
            }
            finally
            {
                ReleaseLock(_lockTaken);
            }
        }

        private void RetrieveTimeoutFromConfig()
        {
            if (!Int32.TryParse(_configuration["AppSettings:CacheMonitorTimeout"], out int cacheMonitorTimeout))
            {
                cacheMonitorTimeout = DefaultCacheMonitorTimeout;
            }

            _timeout = TimeSpan.FromMilliseconds(cacheMonitorTimeout);
        }

        private byte[] CreateNewPackageOnlyIfNotCreatedEarlier(ZipFileInfo zipFileInfo, string zipFilesFolder,
            bool forceRefresh)
        {
            byte[] result;
            if (forceRefresh || !_memoryCache.TryGetValue(zipFileInfo.FileName, out result))
            {
                result = BuildAndSavePackage(zipFileInfo, zipFilesFolder);
            }

            return result;
        }

        private byte[] BuildAndSavePackage(ZipFileInfo zipFileInfo, string zipFilesFolder)
        {
            var result = _zipFileInfoService.ReadPackage(zipFileInfo, zipFilesFolder);
            SavePackageInMemoryCache(zipFileInfo, result);
            return result;
        }

        private static void ReleaseLock(bool lockTaken)
        {
            if (lockTaken)
            {
                Monitor.Exit(_cacheLock);
            }
        }

        private void SavePackageInMemoryCache(ZipFileInfo zipFileInfo, byte[] result)
        {
            DateTimeOffset expirationDate = GetExpirationDate(zipFileInfo.PackageDate);

            SavePackageInMemoryCacheWithExpirationDate(zipFileInfo.FileName, result, expirationDate);
        }

        private void SavePackageInMemoryCacheWithExpirationDate(object key, object result,
            DateTimeOffset expirationDate)
        {
            _memoryCache.Set(key, result, expirationDate);
            _logger.LogInformation(
                $"Created package in memory cache with key: {key} Expiration date: {expirationDate}");
        }

        private DateTimeOffset GetExpirationDate(DateTime packageDate)
        {
            var previousDayExpiration = Math.Min(DateTime.UtcNow.Ticks, packageDate.Ticks);
            return new DateTimeOffset(new DateTime(previousDayExpiration, DateTimeKind.Utc) + _fileCachingTimespan);
        }

    }
}
