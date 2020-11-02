using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web.Http;
using DIGNDB.App.SmitteStop.API.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.IO;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/logging")]
    [Route("logging")]
    public class LoggingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoggingController> _logger;
        private readonly ILogMessageValidator _logMessageValidator;
        private readonly IDictionary<string, string> _logMobilePatternsDictionary;
        private readonly log4net.ILog _loggerMobile;
        private readonly int _maxTextFieldLength;
        private readonly bool _logEndpointOverride;

        public LoggingController(ILogMessageValidator logMessageValidator, ILogger<LoggingController> logger,
           IConfiguration configuration)
        {
            _logMessageValidator = logMessageValidator;
            _configuration = configuration;
            _logger = logger;
            _logMobilePatternsDictionary = InitializePatternDictionary(_configuration);
            _loggerMobile = MobileLoggerFactory.GetLogger();
            int.TryParse(configuration["LogValidationRules:maxTextFieldLength"], out _maxTextFieldLength);
            bool.TryParse(configuration["AppSettings:logEndpointOverride"], out _logEndpointOverride);
        }

        [HttpPost]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        [Route("logMessages")]
        public async Task<IActionResult> UploadMobileLogs()
        {
            var requestBody = String.Empty;
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
                    _logMessageValidator.SanitizeAndShortenTextFields(logMessageMobile, _maxTextFieldLength == 0 ? 500: _maxTextFieldLength);
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

                _logger.LogDebug("Log message saved successfuly");
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
                _logger.LogError("An error occured while trying to save logs " + ex);
                return StatusCode(500);
            }
        }
        private IDictionary<string, string> InitializePatternDictionary(IConfiguration configuration)
        {
            var logMobilePatternsDictionary = new Dictionary<string, string>();
            logMobilePatternsDictionary.Add("severityRegex", configuration["LogValidationRules:severityRegex"]);
            logMobilePatternsDictionary.Add("positiveNumbersRegex", configuration["LogValidationRules:positiveNumbersRegex"]);
            logMobilePatternsDictionary.Add("buildVersionRegex", configuration["LogValidationRules:buildVersionRegex"]);
            logMobilePatternsDictionary.Add("operationSystemRegex", configuration["LogValidationRules:operationSystemRegex"]);
            logMobilePatternsDictionary.Add("deviceOSVersionRegex", configuration["LogValidationRules:deviceOSVersionRegex"]);
            return logMobilePatternsDictionary;
        }
    }
}
