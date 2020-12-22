using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.EuGateway
{
    public class DownloadTemporaryExposureKeysEuGatewayJob
    {
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<DownloadTemporaryExposureKeysEuGatewayJob> _logger;

        public DownloadTemporaryExposureKeysEuGatewayJob(IEuGatewayService euGatewayService, ILogger<DownloadTemporaryExposureKeysEuGatewayJob> logger)
        {
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke(int fromLastNumberOfDays)
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(DownloadTemporaryExposureKeysEuGatewayJob)} started at {startTime}");

            _euGatewayService.DownloadKeysFromGateway(fromLastNumberOfDays);

            _logger.LogInformation($"# Job Ended : {nameof(DownloadTemporaryExposureKeysEuGatewayJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
