using DIGNDB.APP.SmitteStop.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CovidStatisticsFileValidator
    {
        private readonly ILogger<CovidStatisticsFileValidator> _logger;
        private readonly AppSettingsConfig _appSettingsConfig;

        public CovidStatisticsFileValidator(ILogger<CovidStatisticsFileValidator> logger, AppSettingsConfig appSettingsConfig)
        {
            _logger = logger;
            _appSettingsConfig = appSettingsConfig;
        }

        public string VerifyOrThrow(out IFormFile file, HttpRequest request)
        {
            var gitHubSettings = _appSettingsConfig.GitHubSettings;
            var folderPath = gitHubSettings.GitHubStatisticsZipFileFolder;

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

            var testedFileNamePatten = gitHubSettings.TestedFileNamePattern;
            var hospitalAdmissionFileNamePatten = gitHubSettings.HospitalAdmissionFileNamePattern;
            var vaccinationFileNamePatten = gitHubSettings.VaccinationFileNamePattern;

            var testedFileNameMatches = Regex.Matches(fileName, testedFileNamePatten);
            var hospitalAdmissionFileNameMatches = Regex.Matches(fileName, hospitalAdmissionFileNamePatten);
            var vaccinationFileNameMatches = Regex.Matches(fileName, vaccinationFileNamePatten);

            var testedMatch = testedFileNameMatches.Count == 1;
            var hospitalAdmissionMatch = hospitalAdmissionFileNameMatches.Count == 1;
            var vaccinationMatch = vaccinationFileNameMatches.Count == 1;

            if (testedMatch || hospitalAdmissionMatch || vaccinationMatch)
            {
                return destinationPath;
            }

            throw new GitHubControllerServerErrorException("File name not acceptable");
        }
    }
}
