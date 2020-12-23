using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.EuGateway
{
    public class DownloadTemporaryExposureKeysEuGatewayJob
    {
        private readonly DownloadKeysFromGatewayJobConfig _downloadKeysFromGatewayJobConfig;
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<DownloadTemporaryExposureKeysEuGatewayJob> _logger;

        public DownloadTemporaryExposureKeysEuGatewayJob(DownloadKeysFromGatewayJobConfig downloadKeysFromGatewayJobConfig, IEuGatewayService euGatewayService, ILogger<DownloadTemporaryExposureKeysEuGatewayJob> logger)
        {
            _downloadKeysFromGatewayJobConfig = downloadKeysFromGatewayJobConfig;
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke()
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(DownloadTemporaryExposureKeysEuGatewayJob)} started at {startTime}");

            _euGatewayService.DownloadKeysFromGateway(_downloadKeysFromGatewayJobConfig.MaximumNumberOfDaysBack);

            _logger.LogInformation($"# Job Ended : {nameof(DownloadTemporaryExposureKeysEuGatewayJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
