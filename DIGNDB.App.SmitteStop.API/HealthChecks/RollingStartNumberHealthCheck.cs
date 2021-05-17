using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.HealthChecks
{
    /// <summary>
    /// Implementation of IHealthCheck for log files health checks
    /// </summary>
    [Authorize(Policy = HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)]
    public class RollingStartNumberHealthCheck : IHealthCheck
    {
        private const string Description = "Health check for rolling start number";
        private const string QueryValue = "batchSize";

        private readonly ILogger<RollingStartNumberHealthCheck> _logger;
        private readonly ITemporaryExposureKeyRepository _temporaryExposureKeyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="temporaryExposureKeyRepository"></param>
        /// <param name="httpContextAccessor"></param>
        public RollingStartNumberHealthCheck(ILogger<RollingStartNumberHealthCheck> logger,
            ITemporaryExposureKeyRepository temporaryExposureKeyRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _temporaryExposureKeyRepository = temporaryExposureKeyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Checks access to log files and that they are written to every day
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Health check {Startup.RollingStartNumberPattern}");

            var batchSize = 10;
            var query = _httpContextAccessor.HttpContext.Request.Query;
            if (query.ContainsKey(QueryValue))
            {
                query.TryGetValue(QueryValue, out var values);
                batchSize = int.Parse(values.ToString());
            }

            var status = HealthStatus.Healthy;
            var data = new Dictionary<string, object>();

            try
            {
                var temporaryExposureKeys = _temporaryExposureKeyRepository.GetAllKeysNextBatch(0, batchSize);
                foreach (var key in temporaryExposureKeys)
                {
                    var rollingStartNumber = key.RollingStartNumber;
                    var isMidnight = IsMidnight(rollingStartNumber);
                    if (isMidnight)
                    {
                        continue;
                    }

                    status = HealthStatus.Unhealthy;
                    data.Add($"Entry {key.Id} rollingStartNumber not midnight", $"{rollingStartNumber}");
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"{e.Message} - {e.StackTrace}";
                _logger.LogError(errorMessage);

                status = HealthStatus.Unhealthy;
                data.Add($"Error in data retrieval {DateTime.Now}", errorMessage);

                return Task.FromResult(new HealthCheckResult(
                    status,
                    Description,
                    e,
                    data));
            }

            return Task.FromResult(new HealthCheckResult(
                status,
                Description,
                data: data));
        }
        
        private static bool IsMidnight(long rollingStartNumber)
        {
            var midnight = ((DateTimeOffset) DateTimeOffset.FromUnixTimeSeconds(rollingStartNumber).UtcDateTime
                .ToUniversalTime()
                .Date).ToUnixTimeSeconds();

            return rollingStartNumber == midnight;
        }
    }
}
