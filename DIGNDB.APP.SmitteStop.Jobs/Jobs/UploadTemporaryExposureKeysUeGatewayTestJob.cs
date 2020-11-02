using DIGNDB.APP.SmitteStop.Jobs.Config;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class UploadTemporaryExposureKeysUeGatewayTestJob
    {
        private readonly IEuGatewayService _euGatewayService;
        private readonly ILogger<UploadTemporaryExposureKeysUeGatewayJob> _logger;

        public UploadTemporaryExposureKeysUeGatewayTestJob(IEuGatewayService euGatewayService, ILogger<UploadTemporaryExposureKeysUeGatewayJob> logger)
        {
            _euGatewayService = euGatewayService;
            _logger = logger;
        }

        public void Invoke(int fromLastNumberOfDays, int bachSize, int batchLimit)
        {
            var startTime = DateTimeOffset.UtcNow;
            _logger.LogInformation($"# Starting Job : {nameof(UploadTemporaryExposureKeysUeGatewayTestJob)} started at {startTime}");

            _euGatewayService.UploadKeysToTheGateway(
                uploadKeysAgeLimitInDays: fromLastNumberOfDays,
                batchSize: bachSize,
                batchCountLimit: batchLimit
                );

            _logger.LogInformation($"# Job Ended : {nameof(UploadTemporaryExposureKeysUeGatewayTestJob)} started at {startTime}, ended at {DateTimeOffset.UtcNow}");
        }
    }
}
