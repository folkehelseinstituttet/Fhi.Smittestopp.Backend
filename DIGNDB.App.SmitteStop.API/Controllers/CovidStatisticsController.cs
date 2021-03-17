using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.APP.SmitteStop.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DIGNDB.App.SmitteStop.API.Controllers
{
    [ApiController]
    [Route("uploadStatistics")]

    public class CovidStatisticsUploadController : ControllerBase
    {
        private readonly ILogger<CovidStatisticsUploadController> _logger;
        private readonly AppSettingsConfig _appSettingsConfig;

        public CovidStatisticsUploadController(ILogger<CovidStatisticsUploadController> logger, AppSettingsConfig appSettingsConfig)
        {
            _appSettingsConfig = appSettingsConfig;
            _logger = logger;
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

                try
                {
                    await using Stream fileStream = new FileStream(destinationPath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                    await fileStream.DisposeAsync();

                    _logger.LogInformation($"File uploaded completed successfully: {file.FileName}");

                    return Ok("File uploaded");
                }
                catch (Exception e)
                {
                    var errorMessage = "Server error: Error when trying to save zip file";
                    _logger.LogError(errorMessage, e);
                    throw new GitHubControllerServerErrorException(errorMessage, e);
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

            var testedFileNamePatten = gitHubSettings.TestedFileNamePattern;
            var hospitalAdmissionFileNamePatten = gitHubSettings.HospitalAdmissionFileNamePattern;
            var vaccinationFileNamePatten = gitHubSettings.VaccinationFileNamePattern;

            var testedFileNameMatches = Regex.Matches(fileName, testedFileNamePatten);
            var hospitalAdmissionFileNameMatches = Regex.Matches(fileName, hospitalAdmissionFileNamePatten);
            var vaccinationFileNameMatches = Regex.Matches(fileName, vaccinationFileNamePatten);

            var testedMatch = testedFileNameMatches.Count == 1;
            var hospitalAdmissionMatch = hospitalAdmissionFileNameMatches.Count == 1;
            var vaccinationMatch = vaccinationFileNameMatches.Count == 1;

            return testedMatch || hospitalAdmissionMatch || vaccinationMatch;
        }
    }
}
