﻿using DIGNDB.App.SmitteStop.API.HealthCheckAuthorization;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.API.HealthChecks
{
    /// <summary>
    /// Implementation of IHealthCheck for log files health checks
    /// </summary>
    [Authorize(Policy = HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme)]
    public class LogFilesHealthCheck : IHealthCheck
    {
        private const string Description = "Health check for log files";

        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly ILogger<LogFilesHealthCheck> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileSystem _fileSystem;
        private readonly IPathHelper _pathHelper;

        private readonly string _logFilesDatePattern;
        private readonly Regex _apiRegex;
        private readonly Regex _jobsRegex;
        private readonly string _mobileLogFilesDatePattern;
        private readonly Regex _mobileRegex;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="appSettingsConfig"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="fileSystem"></param>
        /// <param name="pathHelper"></param>
        public LogFilesHealthCheck(AppSettingsConfig appSettingsConfig, ILogger<LogFilesHealthCheck> logger, IHttpContextAccessor httpContextAccessor, IFileSystem fileSystem, IPathHelper pathHelper)
        {
            _appSettingsConfig = appSettingsConfig;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _fileSystem = fileSystem;
            _pathHelper = pathHelper;

            // Initialize Health Check AppSettings
            var apiRegex = _appSettingsConfig.HealthCheckSettings.ApiRegex;
            _apiRegex = new Regex(apiRegex);
            var jobsRegex = _appSettingsConfig.HealthCheckSettings.JobsRegex;
            _jobsRegex = new Regex(jobsRegex);
            var mobileRegex = _appSettingsConfig.HealthCheckSettings.MobileRegex;
            _mobileRegex = new Regex(mobileRegex);
            _logFilesDatePattern = _appSettingsConfig.HealthCheckSettings.LogFilesDatePattern;
            _mobileLogFilesDatePattern = _appSettingsConfig.HealthCheckSettings.MobileLogFilesDatePattern;
        }

        /// <summary>
        /// Checks access to log files and that they are written to every day
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Health check {Startup.LogFilesPattern}");

            var status = HealthStatus.Healthy;
            var data = new Dictionary<string, object>();

            try
            {
                // Api log files
                var apiLogFilesNamePrefix = _appSettingsConfig.LogsApiPath;
                CheckLogFiles(apiLogFilesNamePrefix, _apiRegex, ref status, data, _logFilesDatePattern);

                // Jobs log files
                var query = _httpContextAccessor.HttpContext.Request.Query;
                if (QueryContainsWfe01AndMachineIsWfe01(query))
                {
                    var jobsLogFilesNamePrefix = _appSettingsConfig.LogsJobsPath;
                    CheckLogFiles(jobsLogFilesNamePrefix, _jobsRegex, ref status, data, _logFilesDatePattern);
                }

                // Mobile log files
                var mobileLogFilesNamePrefix = _appSettingsConfig.LogsMobilePath; // see also log4net.config where this is configured
                CheckLogFiles(mobileLogFilesNamePrefix, _mobileRegex, ref status, data, _mobileLogFilesDatePattern);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} - {e.StackTrace}");
                return Task.FromResult(new HealthCheckResult(
                    status,
                    Description,
                    e,
                    data));
            }

            return Task.FromResult(new HealthCheckResult(
                status,
                Description,
                data: data));
        }

        private void CheckLogFiles(string logFilesNamePrefix, Regex logFilesRegex, ref HealthStatus status,
            IDictionary<string, object> data, string datePattern)
        {
            var logFilesDirectoryName = _pathHelper.GetDirectoryName(logFilesNamePrefix);
            if (!_fileSystem.DirectoryExists(logFilesDirectoryName))
            {
                status = HealthStatus.Unhealthy;
                data.Add($"Could not find log files for {logFilesNamePrefix}", $"Folder for {logFilesNamePrefix} does not exist");
                //var keyValue = new KeyValuePair<string, object>("State", $"Could not find log files for {logFilesNamePrefix}: Folder for {logFilesNamePrefix} does not exist");
                //data.Add(keyValue);
            }
            else if (logFilesDirectoryName == null)
            {
                status = HealthStatus.Unhealthy;
                data.Add($"Argument {nameof(logFilesDirectoryName)} is null or is a root directory {DateTime.Now}", "");
            }
            else
            {
                var logFilesDirectory = new DirectoryInfo(logFilesDirectoryName);
                var orderedLogFiles = logFilesDirectory.GetFiles().OrderByDescending(f => f.LastWriteTime)
                    .Where(n => logFilesRegex.IsMatch(n.Name));
                var logFiles = orderedLogFiles.ToList();
                if (!logFiles.Any()) // No log files
                {
                    status = HealthStatus.Unhealthy;
                    data.Add($"No log files for {logFilesNamePrefix}", "");
                }
                else // No log files today
                {
                    var today = DateTime.Today.ToString(datePattern);
                    var fileForToday = logFiles.First().Name.Contains(today);
                    if (!fileForToday)
                    {
                        status = HealthStatus.Unhealthy;
                        data.Add($"No log file for today {logFilesNamePrefix}", "");
                    }
                }
            }
        }

        private bool QueryContainsWfe01AndMachineIsWfe01(IQueryCollection query)
        {
            query.TryGetValue("server", out var server);
            if (!server.Any())
            {
                return false;
            }

            var serverParameter = server[0].ToLower();

            var server1Name = _appSettingsConfig.HealthCheckSettings.Server1Name.ToLower();
            var queryContainsServer1Name = serverParameter == server1Name;

            var name = Environment.MachineName.ToLower();
            var isServer1 = name.Contains(server1Name);

            _logger.LogInformation($"|Health check log files| Server name: {name}; Server1Name: {server1Name}; ServerParameter: {serverParameter}; Field for server name 'isWfe01' value: {isServer1}; Query contains 'wfe01': {queryContainsServer1Name}; Query: {query.Keys}");

            // true IF query contains server name AND request is received on server 1
            return queryContainsServer1Name && isServer1;
        }
    }
}
