using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class UploadTemporaryExposureKeysUeGatewayJob
    {
        private readonly UploadKeysToGatewayJobConfig _config;
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<UploadTemporaryExposureKeysUeGatewayJob> _logger;

        public UploadTemporaryExposureKeysUeGatewayJob(UploadKeysToGatewayJobConfig config, IEuGatewayService euGatewayService, ILogger<UploadTemporaryExposureKeysUeGatewayJob> logger)
        {
            _config = config;
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke()
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(UploadTemporaryExposureKeysUeGatewayJob)} started at {startTime}");

            _euGatewayService.UploadKeysToTheGateway(uploadKeysAgeLimitInDays: _config.UploadKeysAgeLimitInDays, batchSize: _config.BatchSize);

            _logger.LogInformation($"# Job Ended : {nameof(UploadTemporaryExposureKeysUeGatewayJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
