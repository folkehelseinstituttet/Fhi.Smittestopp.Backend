using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace DIGNDB.App.SmitteStop.API.V3.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("api/v{version:apiVersion}/diagnostickeys")]
    public class DiagnosticKeyControllerV3 : ControllerBase
    {
        private const string ApiVersion = "3";

        private readonly IAddTemporaryExposureKeyService _addTemporaryExposureKeyService;
        private readonly IExposureKeyValidator _exposureKeyValidator;
        private readonly ILogger _logger;
        private readonly IExposureConfigurationService _exposureConfigurationService;
        private readonly KeyValidationConfiguration _keyValidationConfig;
        private readonly IZipFileInfoService _zipFileInfoService;
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly ICacheOperationsV3 _cacheOperations;



        public DiagnosticKeyControllerV3(
            ILogger<DiagnosticKeyControllerV3> logger,
            IExposureKeyValidator exposureKeyValidator,
            IExposureConfigurationService exposureConfigurationService,
            KeyValidationConfiguration keyValidationConfig,
            IAddTemporaryExposureKeyService addTemporaryExposureKeyService,
            IZipFileInfoService zipFileInfoService,
            AppSettingsConfig appSettingsConfig,
            ICacheOperationsV3 cacheOperations)
        {
            _cacheOperations = cacheOperations;
            _exposureKeyValidator = exposureKeyValidator;
            _logger = logger;
            _zipFileInfoService = zipFileInfoService;
            _appSettingsConfig = appSettingsConfig;
            _exposureConfigurationService = exposureConfigurationService;
            _keyValidationConfig = keyValidationConfig;
            _addTemporaryExposureKeyService = addTemporaryExposureKeyService;
        }

        #region Smitte|stop API
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        [HttpGet("exposureconfiguration")]
        public ActionResult GetExposureConfiguration()
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
        //[TypeFilter(typeof(AuthorizationAttribute))]
        public async Task<IActionResult> UploadDiagnosisKeys()
        {
            var requestBody = String.Empty;

            try
            {
                _logger.LogInformation("UploadDiagnosisKeys endpoint called");
                TemporaryExposureKeyBatchDto parameters = await GetRequestParameters();
                await _addTemporaryExposureKeyService.CreateKeysInDatabase(parameters, KeySource.SmitteStopApiVersion3);

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


        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        [HttpGet("{packageName}")]
        public async Task<IActionResult> DownloadDiagnosisKeysFile(string packageName)
        {
            _logger.LogInformation("DownloadDiagnosisKeysFile endpoint called");
            try
            {
                if (packageName == "today")
                    packageName = ReplacePackageNameWithToday();

                ZipFileInfo packageInfo = _zipFileInfoService.CreateZipFileInfoFromPackageName(packageName);
                string zipFilesFolder = _appSettingsConfig.ZipFilesFolder;

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
                    byte[] zipFileContent = null;
                    bool invalidateCache = false;
                    if (Request.Headers.ContainsKey("Cache-Control") && Request.Headers["Cache-Control"] == "no-cache")
                    {
                        invalidateCache = true;
                        zipFileContent = await _cacheOperations.GetCacheValue(packageInfo, zipFilesFolder, invalidateCache);
                    }
                    else
                    {
                        zipFileContent = await _cacheOperations.GetCacheValue(packageInfo, zipFilesFolder, invalidateCache);
                    }
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

        private string ReplacePackageNameWithToday()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
            stringBuilder.Append("_1");
            stringBuilder.Append($"_{_appSettingsConfig.OriginCountryCode.ToLower()}");
            stringBuilder.Append(".zip");

            return stringBuilder.ToString();
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
            _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameters, _keyValidationConfig);

            return parameters;
        }
    }
    #endregion
}
