using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.EuGateway
{
    public class UploadTemporaryExposureKeysEuGatewayJob
    {
        private readonly UploadKeysToGatewayJobConfig _config;
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<UploadTemporaryExposureKeysEuGatewayJob> _logger;

        public UploadTemporaryExposureKeysEuGatewayJob(UploadKeysToGatewayJobConfig config, IEuGatewayService euGatewayService, ILogger<UploadTemporaryExposureKeysEuGatewayJob> logger)
        {
            _config = config;
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke()
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(UploadTemporaryExposureKeysEuGatewayJob)} started at {startTime}");

            _euGatewayService.UploadKeysToTheGateway(uploadKeysAgeLimitInDays: _config.UploadKeysAgeLimitInDays, batchSize: _config.BatchSize);

            _logger.LogInformation($"# Job Ended : {nameof(UploadTemporaryExposureKeysEuGatewayJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
