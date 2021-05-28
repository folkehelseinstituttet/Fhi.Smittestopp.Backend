using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.HealthChecks
{
    /// <summary>
    /// Implementation of IHealthCheck for log files health checks
    /// </summary>
    [Authorize(Policy = HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)]
    public class NumbersTodayHealthCheck : TimedHealthCheck, IHealthCheck
    {
        private const string Description = "Numbers today health check inspects presence of and accessibility to files containing number for today including database update";

        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly ILogger<NumbersTodayHealthCheck> _logger;
        private readonly IFileSystem _fileSystem;
        private readonly ICovidStatisticsRepository _covidStatisticsRepository;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="apiConfiguration"></param>
        /// <param name="logger"></param>
        /// <param name="fileSystem"></param>
        /// <param name="covidStatisticsRepository"></param>
        public NumbersTodayHealthCheck(AppSettingsConfig apiConfiguration, ILogger<NumbersTodayHealthCheck> logger,
            IFileSystem fileSystem, ICovidStatisticsRepository covidStatisticsRepository)
        {
            _appSettingsConfig = apiConfiguration;
            _logger = logger;
            _fileSystem = fileSystem;
            _covidStatisticsRepository = covidStatisticsRepository;
        }

        /// <summary>
        /// Checks access to log files and that they are written to every day
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Health check {Startup.NumbersTodayPattern}");

            var status = HealthStatus.Healthy;
            var data = new Dictionary<string, object>();

            var hour = _appSettingsConfig.HealthCheckSettings.NumbersTodayCallAfter24Hour;
            if (TooEarly(hour, _logger))
            {
                var key = $"Too early to check numbers today {DateTime.Now}";
                data.Add(key, $"Configured value is {hour}");
                return await Task.FromResult(new HealthCheckResult(
                    status,
                    Description,
                    data: data));
            }

            // Check directory exists
            var directoryPath = _appSettingsConfig.GitHubSettings.GitHubStatisticsZipFileFolder;
            if (!_fileSystem.DirectoryExists(directoryPath))
            {
                status = HealthStatus.Unhealthy;
                data.Add("Directory for SSI statistics does not exist", directoryPath);
            }

            // Check latest file is from today
            var directory = new DirectoryInfo(directoryPath);
            var latestFileInfo = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            var today = DateTime.Today.ToString("yyyy_MM_dd");
            if (!latestFileInfo.Name.Contains(today))
            {
                status = HealthStatus.Unhealthy;
                data.Add($"SSI statistics file for today does not exist. Latest file is {latestFileInfo.Name}", directoryPath);
            }

            // Check numbers have been stored in database
            try
            {
                // check infection numbers
                var newestEntry = await _covidStatisticsRepository.GetNewestEntryAsync();
                var entryDate = newestEntry.EntryDate;
                var entryDateString = entryDate.ToString("yyyy_MM_dd");
                if (!entryDateString.Contains(today))
                {
                    status = HealthStatus.Unhealthy;
                    data.Add($"SSI statistics infection entry in database is not from today {DateTime.Now}", $"Latest entry is from {entryDate}");
                }

                // check vaccine numbers
                var newestVaccineEntry = await _covidStatisticsRepository.GetNewestEntryAsync();
                var vaccineEntryDate = newestVaccineEntry.EntryDate;
                var vaccineEntryDateString = vaccineEntryDate.ToString("yyyy_MM_dd");
                if (!vaccineEntryDateString.Contains(today))
                {
                    status = HealthStatus.Unhealthy;
                    data.Add($"SSI statistics vaccine entry in database is not from today {DateTime.Now}", $"Latest entry is from {entryDate}");
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"{e.Message} - {e.StackTrace}";
                _logger.LogError(errorMessage);

                status = HealthStatus.Unhealthy;
                data.Add($"Error in data retrieval {DateTime.Now}", errorMessage);

                return await Task.FromResult(new HealthCheckResult(
                    status,
                    Description,
                    e,
                    data));
            }

            return await Task.FromResult(new HealthCheckResult(
                status,
                Description,
                data: data));
        }
    }
}
