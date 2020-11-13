using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DIGNDB.App.SmitteStop.API
{
    [ApiController]
    [ApiVersion(_apiVersion)]
    [Route("v{version:apiVersion}/diagnostickeys")]
    public class DiagnosticKeysControllerV2 : ControllerBase
    {
        private readonly IAppleService _appleService;
        private readonly IAddTemporaryExposureKeyService _addTemporaryExposureKeyService;
        private readonly IConfiguration _configuration;
        private readonly IExposureKeyValidator _exposureKeyValidator;
        private readonly ILogger _logger;
        private readonly IExposureConfigurationService _exposureConfigurationService;
        private readonly KeyValidationConfiguration _keyValidationConfig;
        private readonly IZipFileInfoService _zipFileInfoService;
        private readonly AppSettingsConfig _appSettingsConfig;

        private const string _apiVersion = "2";

        public DiagnosticKeysControllerV2(ILogger<DiagnosticKeysControllerV2> logger, IAppleService appleService,
            IConfiguration configuration, IExposureKeyValidator exposureKeyValidator,
            IExposureConfigurationService exposureConfigurationService, KeyValidationConfiguration keyValidationConfig,
            IAddTemporaryExposureKeyService addTemporaryExposureKeyService, IZipFileInfoService zipFileInfoService, AppSettingsConfig appSettingsConfig)
        {
            _configuration = configuration;
            _exposureKeyValidator = exposureKeyValidator;
            _logger = logger;
            _zipFileInfoService = zipFileInfoService;
            _appSettingsConfig = appSettingsConfig;
            _appleService = appleService;
            _exposureConfigurationService = exposureConfigurationService;
            _keyValidationConfig = keyValidationConfig;
            _addTemporaryExposureKeyService = addTemporaryExposureKeyService;
        }

        #region Smitte|stop API
        [HttpGet]
        [Route("exposureconfiguration")]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public  ActionResult GetExposureConfiguration()
        {
            try
            {
                _logger.LogInformation("GetExposureConfiguration endpoint called");
                var exposureConfiguration = _exposureConfigurationService.GetConfigurationR1_2();
                _logger.LogInformation("ExposureConfiguration fetched successfully");
                return Ok(exposureConfiguration);
            }
            catch (ArgumentException e)
            {
                _logger.LogError("Error: " + e);
                return BadRequest("Invalid exposure configuration or uninitialized");
            }
            catch (Exception e)
            {
                _logger.LogError("Error returning config:" + e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public async Task<IActionResult> UploadDiagnosisKeys()
        {

            var requestBody = String.Empty;

            try
            {
                _logger.LogInformation("UploadDiagnosisKeys endpoint called");
                TemporaryExposureKeyBatchDto parameters = await GetRequestParameters();
                await _addTemporaryExposureKeyService.CreateKeysInDatabase(parameters);

                _logger.LogInformation("Keys uploaded successfully");
                return Ok();
            }
            catch (JsonException je)
            {

                _logger.LogError($"Incorrect JSON format: { je}  [Deserialized request]: {requestBody}");
                return BadRequest($"Incorrect JSON format: {je.Message}");
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Incorrect input format: " + ae);
                return BadRequest("Incorrect input format: " + ae.Message);
            }
            catch (SqlException e)
            {
                _logger.LogError("Error occurred when uploading keys to the database." + e);
                return StatusCode(500, "Error occurred when uploading keys to the database.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error uploading keys:" + e);
                return StatusCode(500);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="packageName">A timestamp in the format of "yyyy-mm-dd"</param>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        [Route("{packageName}")]
        public IActionResult DownloadDiagnosisKeysFile(string packageName)
        {
            _logger.LogInformation("DownloadDiagnosisKeysFile endpoint called");
            try
            {
                ZipFileInfo packageInfo = _zipFileInfoService.CreateZipFileInfoFromPackageName(packageName);
                string zipFilesFolder = _configuration["ZipFilesFolder"];

                _logger.LogInformation("Package Date: " + packageInfo.PackageDate);
                _logger.LogInformation("Add days: " + DateTime.UtcNow.Date.AddDays(-14));
                _logger.LogInformation("Utc now:" + DateTime.UtcNow);

                if (!IsDateValid(packageInfo.PackageDate, packageName))
                {
                    return BadRequest("Package Date is invalid");
                }

                var packageExists = _zipFileInfoService.CheckIfPackageExists(packageInfo, zipFilesFolder);
                if (packageExists)
                {
                    var zipFileContent = _zipFileInfoService.ReadPackage(packageInfo, zipFilesFolder);
                    var currentBatchNumber = packageInfo.BatchNumber;
                    packageInfo.BatchNumber++;
                    var nextPackageExists = _zipFileInfoService.CheckIfPackageExists(packageInfo, zipFilesFolder);

                    AddResponseHeader(nextPackageExists, currentBatchNumber);
                    _logger.LogInformation("Zip package fetched successfully");
                    return File(zipFileContent, System.Net.Mime.MediaTypeNames.Application.Zip);
                }
                else
                {
                    _logger.LogInformation("Package does not exist");
                    return NoContent();
                }
            }
            catch (FormatException e)
            {
                _logger.LogError("Error when parsing data: " + e);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error when downloading package: " + e);
                return StatusCode(500);
            }
        }

        #endregion
        #region Private methods
        private void AddResponseHeader(bool nextPackageExists, int lastBatchNumber)
        {
            Response.Headers.Add("nextBatchExists", nextPackageExists.ToString());
            Response.Headers.Add("lastBatchReturned", lastBatchNumber.ToString());
        }

        private bool IsDateValid(DateTime packageDate, string packageName)
        {
            if (packageDate < DateTime.UtcNow.Date.AddDays(-14) || packageDate > DateTime.UtcNow)
            {
                _logger.LogError($"Package Date is invalid date: {packageDate} packageName: {packageName}");
                return false;
            }
            return true;
        }

        private async Task<string> ReadRequestBody()
        {
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private async Task<TemporaryExposureKeyBatchDto> GetRequestParameters()
        {
            string requestBody = (await ReadRequestBody());

            var parameters = JsonSerializer.Deserialize<TemporaryExposureKeyBatchDto>(requestBody);
            _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameters, _keyValidationConfig, _logger);
            if (_appSettingsConfig.DeviceVerificationEnabled)
            {
                await _exposureKeyValidator.ValidateDeviceVerificationPayload(parameters, _appleService, _logger);
            }
            return parameters;
        }
    }
    #endregion
}
