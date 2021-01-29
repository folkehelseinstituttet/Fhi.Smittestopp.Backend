using DIGNDB.App.SmitteStop.Domain.Dto;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface ICacheOperations
    {
        /// <summary>
        /// Gets the oldest file with timestamp larger than given as parameter, along with information whether a newer file is available. If no new files, filebytes should be null
        /// </summary>
        /// <param name="key">The time stamp of the last retrieved file from cache</param>
        /// <param name="cancellationToken">cancellation token passed from the WebAPI call</param>
        /// <returns>A the oldest file with timestamp larger than given and information if a newer file is available. If no new files, filebytes should be null</returns>
        Task<CacheResult> GetCacheValue(DateTime key, bool forceRefresh = false);
    }
}
