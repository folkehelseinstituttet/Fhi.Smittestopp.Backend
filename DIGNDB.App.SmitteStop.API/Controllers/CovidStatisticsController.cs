using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Controllers
{
    [ApiController]
    [Route("uploadStatistics")]

    public class CovidStatisticsUploadController : ControllerBase
    {
        private readonly ILogger<CovidStatisticsUploadController> _logger;
        private readonly AppSettingsConfig _appSettingsConfig;

        public CovidStatisticsUploadController(ILogger<CovidStatisticsUploadController> logger,
            AppSettingsConfig appSettingsConfig)
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
                var folderPath = _appSettingsConfig.GitHubStatisticsZipFileFolder;
                if (!Directory.Exists(folderPath))
                {
                    const string errorMessage = "Server error: The file save directory is not reachable or does not exist";
                    _logger.LogError(errorMessage);

                    throw new GitHubControllerServerErrorException(errorMessage);
                }

                var sentFiles = Request.Form.Files;
                if (sentFiles.Count != 1)
                {
                    throw new GitHubControllerServerErrorException("Files count is not equal to 1");
                }

                var file = sentFiles.First();
                if (file.Length <= 0)
                {
                    throw new GitHubControllerServerErrorException("The file sent was empty");
                }
                
                var destinationPath = Path.Combine(folderPath, file.FileName);
                try
                {
                    await using Stream fileStream = new FileStream(destinationPath, FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    _logger.LogInformation($"File uploaded completed successfully: {file.FileName}");

                    return Ok();
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
    }
}
