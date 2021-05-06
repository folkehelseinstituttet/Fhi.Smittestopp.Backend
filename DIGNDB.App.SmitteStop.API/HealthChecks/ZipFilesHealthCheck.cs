//using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
//using DIGNDB.App.SmitteStop.Core.Contracts;
//using DIGNDB.APP.SmitteStop.API.Config;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace DIGNDB.App.SmitteStop.API.HealthChecks
//{
//    /// <summary>
//    /// Implementation of IHealthCheck for log files health checks
//    /// </summary>
//    [Authorize(Policy = HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)]
//    public class ZipFilesHealthCheck : TimedHealthCheck, IHealthCheck
//    {
//        private const string Description = "Health check for zip files";

//        private readonly ApiConfig _apiConfiguration;
//        private readonly ILogger<ZipFilesHealthCheck> _logger;
//        private readonly IFileSystem _fileSystem;

//        /// <summary>
//        /// Ctor 
//        /// </summary>
//        /// <param name="apiConfiguration"></param>
//        /// <param name="logger"></param>
//        /// <param name="fileSystem"></param>
//        public ZipFilesHealthCheck(ApiConfig apiConfiguration, ILogger<ZipFilesHealthCheck> logger, IFileSystem fileSystem)
//        {
//            _apiConfiguration = apiConfiguration;
//            _logger = logger;
//            _fileSystem = fileSystem;
//        }

//        /// <summary>
//        /// Checks access to log files and that they are written to every day
//        /// </summary>
//        /// <param name="context"></param>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            _logger.LogInformation($"Health check {Startup.ZipFilesPattern}");

//            var status = HealthStatus.Healthy;
//            var data = new Dictionary<string, object>();

//            var hour = _apiConfiguration.HealthCheckSettings.ZipFilesCallAfter24Hour;
//            if (TooEarly(hour, _logger))
//            {
//                var key = $"Too early to check zip files {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
//                data.Add(key, $"Configured value is {hour}");
//                return await Task.FromResult(new HealthCheckResult(
//                    status,
//                    Description,
//                    data: data));
//            }
            
//            try
//            {
//                var directoryPath = _apiConfiguration.ZipFilesFolder;
//                if (!_fileSystem.DirectoryExists(directoryPath))
//                {
//                    status = HealthStatus.Unhealthy;
//                    data.Add("Directory for Zip files does not exist", directoryPath);
//                }
//                else
//                {
//                    CheckLatestFileIsFromToday(directoryPath, ref status, data);
//                }
//            }
//            catch (Exception e)
//            {
//                return await Task.FromResult(new HealthCheckResult(
//                    status,
//                    Description,
//                    e,
//                    data));
//            }

//            return await Task.FromResult(new HealthCheckResult(
//                status,
//                Description,
//                data: data));
//        }

//        private void CheckLatestFileIsFromToday(string directoryPath, ref HealthStatus status, IDictionary<string, object> data)
//        {
//            // Check latest file is from today
//            var directory = new DirectoryInfo(directoryPath);
//            var directories = directory.GetDirectories();
//            if (!directories.Any())
//            {
//                status = HealthStatus.Unhealthy;
//                data.Add("Expected folders 'dk' and 'all' for zip files do not exist", "");
//            }
//            else
//            {
//                foreach (var directoryInfo in directories)
//                {
//                    if (directoryInfo.Name != "dk" && directoryInfo.Name != "all")
//                    {
//                        status = HealthStatus.Unhealthy;
//                        data.Add("Expected folder for zip files does not exist", $"{directoryInfo.Name}");
//                    }

//                    var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
//                    var today = DateTime.Today.ToString("yyyy-MM-dd");
//                    if (latestFile == null)
//                    {
//                        status = HealthStatus.Unhealthy;
//                        data.Add($"No zip file found in folder {directoryInfo.Name}", $"{directoryInfo.Name}");
//                    }
//                    else if (!latestFile.Name.Contains(today))
//                    {
//                        status = HealthStatus.Unhealthy;
//                        data.Add($"No zip file for today has been written to folder {directoryInfo.Name}", $"Latest file is {latestFile.Name}");
//                    }
//                }
//            }
//        }
//    }
//}
