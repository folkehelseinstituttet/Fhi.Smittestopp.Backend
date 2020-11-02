using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Enums;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class MaintenanceDailySummaryJob
    {
        private readonly ITemporaryExposureKeyRepository _temporaryExposureKeyRepository;
        private readonly ILogger<MaintenanceDailySummaryJob> _logger;

        public MaintenanceDailySummaryJob(ITemporaryExposureKeyRepository temporaryExposureKeyRepository, ILogger<MaintenanceDailySummaryJob> logger)
        {
            _temporaryExposureKeyRepository = temporaryExposureKeyRepository;
            _logger = logger;
        }
        public void Invoke(string maintenanceCheckConfig)
        {
            var jobConfig = DeserializeConfig(maintenanceCheckConfig);
            CheckNumberOfDownloadedKeysAndAlarmIfNeeded(jobConfig.GatewayDownloadCheck);
        }

        private void CheckNumberOfDownloadedKeysAndAlarmIfNeeded(GatewayDowloadCheckConfig config)
        {
            var uploadDate = DateTime.UtcNow.AddDays(-config.DayToCheckBeforeTodayOffset.Value).Date;
            var minimalKeyAmmout = config.RiseErrorWhenNumberOfKeysAreBelowNumber.Value;

            var numberOfDownloadedKeys = _temporaryExposureKeyRepository.GetCountOfKeysByUpladedDayAndSource(uploadDate, KeySource.Gateway);
            var message = ($"Number of keys downloaded from EuGateway at {uploadDate}");
            if (numberOfDownloadedKeys < minimalKeyAmmout)
            {
                _logger.LogWarning($"Number of keys downloaded from EuGateway at {uploadDate} seems to be very low (threshold {minimalKeyAmmout}): {numberOfDownloadedKeys}");
            }
        }

        public static string SerializeConfig(DailyMaintenanceCheckJobConfig config)
        {
            return JsonSerializer.Serialize(config);
        }

        public static DailyMaintenanceCheckJobConfig DeserializeConfig(string config)
        {
            return JsonSerializer.Deserialize<DailyMaintenanceCheckJobConfig>(config);
        }
    }
}
