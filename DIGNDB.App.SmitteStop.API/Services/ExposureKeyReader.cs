using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class ExposureKeyReader : IExposureKeyReader
    {
        private readonly IExposureKeyValidator _exposureKeyValidator;
        private readonly KeyValidationConfiguration _keyValidationConfig;
        private readonly ILogger<ExposureKeyReader> _logger;

        public ExposureKeyReader(IExposureKeyValidator exposureKeyValidator, KeyValidationConfiguration keyValidationConfig, ILogger<ExposureKeyReader> logger)
        {
            _exposureKeyValidator = exposureKeyValidator;
            _keyValidationConfig = keyValidationConfig;
            _logger = logger;
        }

        public async Task<TemporaryExposureKeyBatchDto> ReadParametersFromBody(Stream requestBody)
        {
            using var reader = new StreamReader(requestBody);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation($"Logging request body: {requestBody}");

            var parameters = JsonSerializer.Deserialize<TemporaryExposureKeyBatchDto>(body);
            _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameters, _keyValidationConfig);

            return parameters;
        }
    }
}
