using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DIGNDB.App.SmitteStop.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/logging")]
    [Route("api/logging")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;
        private readonly ILogMessageValidator _logMessageValidator;
        private readonly IDictionary<string, string> _logMobilePatternsDictionary;
        private readonly log4net.ILog _loggerMobile;
        private readonly int _maxTextFieldLength;
        private readonly bool _logEndpointOverride;

        public LoggingController(
            ILogMessageValidator logMessageValidator,
            ILogger<LoggingController> logger,
            LogValidationRulesConfig logValidationRulesConfig,
            AppSettingsConfig appSettingsConfig)
        {
            _logMessageValidator = logMessageValidator;

            _logger = logger;
            _logMobilePatternsDictionary = InitializePatternDictionary(logValidationRulesConfig);
            _loggerMobile = MobileLoggerFactory.GetLogger();
            _maxTextFieldLength = logValidationRulesConfig.MaxTextFieldLength;
            _logEndpointOverride = appSettingsConfig.LogEndpointOverride;
        }

        [HttpPost("logMessages")]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> UploadMobileLogs()
        {
            var requestBody = string.Empty;
            try
            {
                if (_logEndpointOverride)
                {
                    return Ok();
                }
                _logger.LogDebug("LogMessage action invoked");
                using (var reader = new StreamReader(HttpContext.Request.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var logMessagesMobileDeserialized = JsonSerializer.Deserialize<LogMessagesMobileCollection>(requestBody, new JsonSerializerOptions() { IgnoreNullValues = false });
                foreach (var logMessageMobile in logMessagesMobileDeserialized.logs)
                {
                    _logMessageValidator.SanitizeAndShortenTextFields(logMessageMobile, _maxTextFieldLength == 0 ? 500 : _maxTextFieldLength);
                    _logMessageValidator.ValidateLogMobileMessagePatterns(logMessageMobile, _logMobilePatternsDictionary);
                    _logMessageValidator.ValidateLogMobileMessageDateTimeFormats(logMessageMobile);
                    switch (logMessageMobile.severity)
                    {
                        case "INFO":
                            _loggerMobile.Info(logMessageMobile);
                            break;
                        case "ERROR":
                            _loggerMobile.Error(logMessageMobile);
                            break;
                        case "WARNING":
                            _loggerMobile.Warn(logMessageMobile);
                            break;
                        default:
                            break;
                    }
                }

                _logger.LogDebug("Log message saved successfully");
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                _logger.LogError("Error when uploading mobile logs" + ex);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"No logs found in body or unable to parse logs data. {ex} [Deserialized request]: {requestBody}");
                return BadRequest("No logs found in body or unable to parse logs data");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while trying to save logs " + ex);
                return StatusCode(500);
            }
        }

        private IDictionary<string, string> InitializePatternDictionary(LogValidationRulesConfig configuration)
        {
            var logMobilePatternsDictionary = new Dictionary<string, string>();
            logMobilePatternsDictionary.Add("severityRegex", configuration.SeverityRegex);
            logMobilePatternsDictionary.Add("positiveNumbersRegex", configuration.PositiveNumbersRegex);
            logMobilePatternsDictionary.Add("buildVersionRegex", configuration.BuildVersionRegex);
            logMobilePatternsDictionary.Add("operationSystemRegex", configuration.OperationSystemRegex);
            logMobilePatternsDictionary.Add("deviceOSVersionRegex", configuration.DeviceOSVersionRegex);
            return logMobilePatternsDictionary;
        }
    }
}
