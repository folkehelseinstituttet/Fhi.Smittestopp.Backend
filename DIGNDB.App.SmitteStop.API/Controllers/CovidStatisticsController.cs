﻿using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Controllers
{
    [ApiController]
    [Route("uploadStatistics")]

    public class CovidStatisticsUploadController : ControllerBase
    {
        private readonly ILogger<CovidStatisticsUploadController> _logger;
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly IStatisticsFileService _statisticsFileService;

        public CovidStatisticsUploadController(ILogger<CovidStatisticsUploadController> logger, AppSettingsConfig appSettingsConfig, IStatisticsFileService statisticsFileService)
        {
            _appSettingsConfig = appSettingsConfig;
            _logger = logger;
            _statisticsFileService = statisticsFileService;
        }

        [HttpPost]
        [ServiceFilter(typeof(GitHubAuthorizationAttribute))]
        public async Task<IActionResult> UploadCovidStatistics()
        {
            _logger.LogInformation("File upload called");
            try
            {
                var destinationPath = VerifyOrThrow(out var file, Request);

                if (System.IO.File.Exists(destinationPath))
                {
                    return Ok("File already uploaded");
                }

                await using Stream fileStream = new FileStream(destinationPath, FileMode.Create);
                try
                {
                    await file.CopyToAsync(fileStream);

                    _logger.LogInformation($"File uploaded completed successfully: {file.FileName}");

                    DeleteOldStatisticsFiles(destinationPath);

                    return Ok("File uploaded");
                }
                catch (Exception e)
                {
                    var errorMessage = "Server error: Error when trying to save zip file";
                    _logger.LogError(errorMessage, e);
                    throw new GitHubControllerServerErrorException(errorMessage, e);
                }
                finally
                {
                    await fileStream.DisposeAsync();
                }
            }
            catch (GitHubControllerServerErrorException e)
            {
                _logger.LogError($"Bad request with covid statistics was sent from GitHub. Error: {e}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                var errorMessage = $"Internal server error when trying to process request with covid statistics from GitHub. Error: {e}";
                _logger.LogError(errorMessage);

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(GitHubAuthorizationAttribute))]
        public async Task<bool> CovidStatisticsAlreadyUploaded()
        {
            _logger.LogInformation("Check covid file uploaded called");

            Request.Query.TryGetValue("filename", out var fileName);
            var folderPath = _appSettingsConfig.GitHubSettings.GitHubStatisticsZipFileFolder;
            if (!CheckFileName(fileName))
            {
                _logger.LogWarning("File name not acceptable");
                return await Task.FromResult(false);
            }

            if (!Directory.Exists(folderPath))
            {
                _logger.LogWarning($"Folder {folderPath} does not exists");
                return await Task.FromResult(false);
            }

            var destinationPath = Path.Combine(folderPath, fileName);
            if (System.IO.File.Exists(destinationPath))
            {
                _logger.LogInformation($"File {fileName} already exists");
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private void DeleteOldStatisticsFiles(string path)
        {
            var gitHubSettings = _appSettingsConfig.GitHubSettings;
            var daysToSaveFiles = gitHubSettings.DaysToSaveFiles;
            var fileDatePattern = gitHubSettings.FileNameDatePattern;
            var fileDateParsingFormat = gitHubSettings.FileNameDateParsingFormat;
            var count = _statisticsFileService.DeleteOldFiles(path, daysToSaveFiles, fileDatePattern, fileDateParsingFormat);

            var file = Path.GetFileName(path);
            _logger.LogInformation($"Old statistics files deleted after upload of {file}: {count}");
        }

        private string VerifyOrThrow(out IFormFile file, HttpRequest request)
        {
            var folderPath = _appSettingsConfig.GitHubSettings.GitHubStatisticsZipFileFolder;

            if (!Directory.Exists(folderPath))
            {
                const string errorMessage = "Server error: The file save directory is not reachable or does not exist";
                _logger.LogError(errorMessage);

                throw new GitHubControllerServerErrorException(errorMessage);
            }

            var sentFiles = request.Form.Files;
            if (sentFiles.Count != 1)
            {
                throw new GitHubControllerServerErrorException("Files count is not equal to 1");
            }

            file = sentFiles.First();
            if (file.Length <= 0)
            {
                throw new GitHubControllerServerErrorException("The file sent was empty");
            }

            var fileName = file.FileName;
            var destinationPath = Path.Combine(folderPath, fileName);

            if (CheckFileName(fileName))
            {
                return destinationPath;
            }

            throw new GitHubControllerServerErrorException("File name not acceptable");
        }

        private bool CheckFileName(string fileName)
        {
            var gitHubSettings = _appSettingsConfig.GitHubSettings;

            var testedFileNamePattern = gitHubSettings.TestedFileNamePattern;
            var hospitalAdmissionFileNamePattern = gitHubSettings.HospitalAdmissionFileNamePattern;
            var vaccinationFileNamePattern = gitHubSettings.VaccinationFileNamePattern;
            var timeLocationFileNamePattern = gitHubSettings.TimeLocationFileNamePattern;
            var locationFileNamePattern = gitHubSettings.LocationFileNamePattern;
            var deathByTimeFileNamePattern = gitHubSettings.DeathByTimeFileNamePattern;

            var testedFileNameMatches = Regex.Matches(fileName, testedFileNamePattern);
            var hospitalAdmissionFileNameMatches = Regex.Matches(fileName, hospitalAdmissionFileNamePattern);
            var vaccinationFileNameMatches = Regex.Matches(fileName, vaccinationFileNamePattern);
            var timeLocationFileNameMatches = Regex.Matches(fileName, timeLocationFileNamePattern);
            var locationFileNameMatches = Regex.Matches(fileName, locationFileNamePattern);
            var deathByTimeFileNameMatches = Regex.Matches(fileName, deathByTimeFileNamePattern);

            var testedMatch = testedFileNameMatches.Count == 1;
            var hospitalAdmissionMatch = hospitalAdmissionFileNameMatches.Count == 1;
            var vaccinationMatch = vaccinationFileNameMatches.Count == 1;
            var timeLocationMatch = timeLocationFileNameMatches.Count == 1;
            var locationMatch = locationFileNameMatches.Count == 1;
            var deathByTimeMatch = deathByTimeFileNameMatches.Count == 1;

            var result = testedMatch || hospitalAdmissionMatch || vaccinationMatch || timeLocationMatch || locationMatch || deathByTimeMatch;
            return result;
        }
    }
}
