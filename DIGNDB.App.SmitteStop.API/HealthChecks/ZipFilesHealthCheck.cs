using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using DIGNDB.App.SmitteStop.Core.Contracts;
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
    public class ZipFilesHealthCheck : TimedHealthCheck, IHealthCheck
    {
        private const string Description = "Health check for zip files";

        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly ILogger<ZipFilesHealthCheck> _logger;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="apiConfiguration"></param>
        /// <param name="logger"></param>
        /// <param name="fileSystem"></param>
        public ZipFilesHealthCheck(AppSettingsConfig apiConfiguration, ILogger<ZipFilesHealthCheck> logger, IFileSystem fileSystem)
        {
            _appSettingsConfig = apiConfiguration;
            _logger = logger;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Checks access to log files and that they are written to every day
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation($"Health check {Startup.ZipFilesPattern}");

            var status = HealthStatus.Healthy;
            var data = new Dictionary<string, object>();
            var daysBefore = _appSettingsConfig.HealthCheckSettings.ZipFilesCheckFilesDaysBefore;

            var hour = _appSettingsConfig.HealthCheckSettings.ZipFilesCallAfter24Hour;
            if (TooEarly(hour, _logger) && daysBefore == 0)
            {
                var key = $"Too early to check zip files {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                data.Add(key, $"Configured value is {hour}");
                return await Task.FromResult(new HealthCheckResult(
                    status,
                    Description,
                    data: data));
            }

            try
            {
                var directoryPath = _appSettingsConfig.ZipFilesFolder;
                if (!_fileSystem.DirectoryExists(directoryPath))
                {
                    status = HealthStatus.Unhealthy;
                    data.Add("Directory for Zip files does not exist", directoryPath);
                }
                else
                {
                    if (daysBefore > 0)
                    {
                        CheckLatestFileDaysBefore(directoryPath, ref status, data, daysBefore);
                    }
                    else
                    {
                        CheckLatestFileDaysBefore(directoryPath, ref status, data, 0);
                    }
                }
            }
            catch (Exception e)
            {
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

        private void CheckLatestFileIsFromToday(string directoryPath, ref HealthStatus status, IDictionary<string, object> data)
        {
            // Check latest file is from today
            var directory = new DirectoryInfo(directoryPath);
            var directories = directory.GetDirectories();
            if (!directories.Any())
            {
                status = HealthStatus.Unhealthy;
                data.Add("Expected folders 'no' and 'all' for zip files do not exist", "");
            }
            else
            {
                foreach (var directoryInfo in directories)
                {
                    if (directoryInfo.Name != "no" && directoryInfo.Name != "all")
                    {
                        status = HealthStatus.Unhealthy;
                        data.Add("Expected folder for zip files does not exist", $"{directoryInfo.Name}");
                    }

                    if (directoryInfo.Name == "all")
                    {
                        var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime)
                            .FirstOrDefault();
                        var today = DateTime.Today.ToString("yyyy-MM-dd");
                        if (latestFile == null)
                        {
                            status = HealthStatus.Unhealthy;
                            data.Add($"No zip file found in folder {directoryInfo.Name}", $"{directoryInfo.Name}");
                        }
                        else if (!latestFile.Name.Contains(today))
                        {
                            status = HealthStatus.Unhealthy;
                            data.Add($"No zip file for today has been written to folder {directoryInfo.Name}",
                                $"Latest file is {latestFile.Name}");
                        }
                    }
                }
            }
        }
        private void CheckLatestFileDaysBefore(string directoryPath, ref HealthStatus status, IDictionary<string, object> data, int daysBefore)
        {
            // Check latest file is from today
            var directory = new DirectoryInfo(directoryPath);
            var directories = directory.GetDirectories();
            if (!directories.Any())
            {
                status = HealthStatus.Unhealthy;
                data.Add("Expected folders 'dk' and 'all' for zip files do not exist", "");
            }
            else
            {
                foreach (var directoryInfo in directories)
                {
                    if (directoryInfo.Name != "no" && directoryInfo.Name != "all")
                    {
                        status = HealthStatus.Unhealthy;
                        data.Add("Expected folder for zip files does not exist", $"{directoryInfo.Name}");
                    }

                    var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                    if (latestFile == null)
                    {
                        status = HealthStatus.Unhealthy;
                        data.Add($"No zip file found in folder {directoryInfo.Name}", $"{directoryInfo.Name}");
                    }
                    else
                    {
                        bool exists = false;
                        for (int i = 0; i <= daysBefore; i++)
                        {
                            var day = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd");
                            if (latestFile.Name.Contains(day))
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            status = HealthStatus.Unhealthy;
                            data.Add(
                                $"No zip files for last {daysBefore + 1} days has been written to folder {directoryInfo.Name}",
                                $"Latest file is {latestFile.Name}");
                        }
                    }
                }
            }
        }
    }
}
