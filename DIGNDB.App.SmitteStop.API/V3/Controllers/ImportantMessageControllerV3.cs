using System;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Config;
using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DIGNDB.App.SmitteStop.API.V3.Controllers
{
    [ApiController]
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/importantinfo")]
    public class ImportantInfoControllerV3 : ControllerBase
    {
        private readonly ILogger<ImportantInfoControllerV3> _logger;
        private readonly IImportantInfoService _importantInfoService;

        public ImportantInfoControllerV3(
            ILogger<ImportantInfoControllerV3> logger, IImportantInfoService importantInfoService)
        {
            _logger = logger;
            _importantInfoService = importantInfoService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ImportantInfoResponse), 200)]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> GetImportantInfo()
        {
            _logger.LogInformation($"{nameof(GetImportantInfo)} endpoint called.");

            try
            {
                using var reader = new StreamReader(HttpContext.Request.Body);
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation($"Logging request body: {HttpContext.Request.Body}");
                var parameters = JsonSerializer.Deserialize<ImportantInfoRequest>(body);

                ImportantInfoList items;
                ImportantInfoResponse res;

                if (!_importantInfoService.ConfigFileExists())
                    return NoContent();

                var message = _importantInfoService.ParseConfig(parameters);

                if (message == null)
                    return NoContent();

                //if (parameters.lastTimeStamp <= DateTime.Parse(message.creationDate))
                //{
                res = new ImportantInfoResponse()
                {
                    text = message.text,
                    //creationDate = message.creationDate,
                    //expirationDate = message.expirationDate,
                    bannerColor = _importantInfoService.GetColor()
                };
                _logger.LogInformation("Important info send");
                return Ok(res);
                //}

                //if (parameters.lastTimeStamp >= DateTime.Parse(message.creationDate) &&
                //    parameters.lastTimeStamp <= DateTime.Parse(message.expirationDate))
                //{
                //    return StatusCode(304);
                //}

                //if (parameters.lastTimeStamp > DateTime.Parse(message.expirationDate))
                //{
                //    return NoContent();
                //}

                return NoContent();
            }
            catch (JsonException je)
            {

                _logger.LogError($"Incorrect JSON format: {je}  [Deserialized request]: {HttpContext.Request.Body}");
                return BadRequest($"Incorrect JSON format: {je.Message}");
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Incorrect input format: " + ae);
                return BadRequest("Incorrect input format: " + ae.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error sending important info:" + e);
                return StatusCode(500);
            }

        }
    }
}