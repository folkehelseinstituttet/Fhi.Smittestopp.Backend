using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CacheOperations : ICacheOperations
    {
        private IMemoryCache _memoryCache;
        private static Object _cacheLock = new object();
        private readonly IPackageBuilderService _cachePackageBuilder;

        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly TimeSpan _previousDayFileCaching;
        private readonly TimeSpan _currentDayFileCaching;

        public CacheOperations(IMemoryCache memoryCache, AppSettingsConfig appSettingsConfig, IPackageBuilderService cachePackageBuilder)
        {
            _memoryCache = memoryCache;
            _appSettingsConfig = appSettingsConfig;
            _cachePackageBuilder = cachePackageBuilder;
            _previousDayFileCaching = appSettingsConfig.PreviousDayFileCaching;
            _currentDayFileCaching = appSettingsConfig.CurrentDayFileCaching;
        }

        public async Task<CacheResult> GetCacheValue(DateTime key, bool forceRefresh = false)
        {
            return await GetValueOrDefault(key, forceRefresh);
        }

        private async Task<CacheResult> GetValueOrDefault(DateTime key, bool forceRefresh)
        {
            if (!_memoryCache.TryGetValue(key, out CacheResult result))
            {
                var timeout = _appSettingsConfig.CacheMonitorTimeout;
                bool lockTaken = false;

                try
                {
                    Monitor.TryEnter(_cacheLock, timeout, ref lockTaken);
                    if (lockTaken)
                    {
                        if (forceRefresh || !_memoryCache.TryGetValue(key, out result))
                        {
                            result = _cachePackageBuilder.BuildPackage(key);

                            //If FUNC returns a null value, cache it for the defined amount of time
                            if (result == null)
                            {
                                //writes the Null placeholder object to the cache.
                                return default;
                            }

                            var previousDayExpiration = Math.Min(DateTime.UtcNow.Ticks, key.Ticks);
                            var absExpiration = new DateTimeOffset(new DateTime(previousDayExpiration) + _previousDayFileCaching);
                            if (key.Date == DateTime.UtcNow.Date)
                            {
                                DateTime midnight = DateTime.UtcNow.AddDays(1).Date;
                                var currentDayExpiration = Math.Min(midnight.Ticks, (DateTime.UtcNow + _currentDayFileCaching).Ticks);
                                absExpiration = new DateTimeOffset(new DateTime(currentDayExpiration));
                            }

                            _memoryCache.Set(key, result, absExpiration);

                            return result;
                        }

                        return result;
                    }
                    else
                    {
                        return new CacheResult() { CouldNotGetLock = true };
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    if (lockTaken)
                    {
                        Monitor.Exit(_cacheLock);
                    }
                }
            }
            return result;
        }
    }
}
