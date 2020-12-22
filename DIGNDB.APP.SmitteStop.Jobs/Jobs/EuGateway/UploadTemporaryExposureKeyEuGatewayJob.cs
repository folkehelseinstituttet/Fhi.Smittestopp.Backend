using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.EuGateway
{
    public class UploadTemporaryExposureKeyEuGatewayJob
    {
        private readonly UploadKeysToGatewayJobConfig _config;
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<UploadTemporaryExposureKeyEuGatewayJob> _logger;

        public UploadTemporaryExposureKeyEuGatewayJob(UploadKeysToGatewayJobConfig config, IEuGatewayService euGatewayService, ILogger<UploadTemporaryExposureKeyEuGatewayJob> logger)
        {
            _config = config;
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke()
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(UploadTemporaryExposureKeyEuGatewayJob)} started at {startTime}");

            _euGatewayService.UploadKeysToTheGateway(uploadKeysAgeLimitInDays: _config.UploadKeysAgeLimitInDays, batchSize: _config.BatchSize);

            _logger.LogInformation($"# Job Ended : {nameof(UploadTemporaryExposureKeyEuGatewayJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
