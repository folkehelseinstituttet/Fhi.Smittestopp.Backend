using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FederationGatewayApi.Services
{
    public class EFGSKeyStoreService : IEFGSKeyStoreService
    {
        private readonly IRiskCalculator _riskCalculator;
        private readonly IEpochConverter _epochConverter;
        private readonly IDaysSinceOnsetOfSymptomsDecoder _daysSinceOnsetOfSymptomsDecoder;
        private readonly IKeyFilter _keyFilter;
        private readonly ITemporaryExposureKeyRepository _tempKeyRepository;
        private readonly ILogger<EFGSKeyStoreService> _logger;

        public EFGSKeyStoreService(IKeyFilter filter,
                                   ITemporaryExposureKeyRepository repository,
                                   ILogger<EFGSKeyStoreService> logger,
                                   IRiskCalculator riskCalculator,
                                   IEpochConverter epochConverter,
                                   IDaysSinceOnsetOfSymptomsDecoder daysSinceOnsetOfSymptomsDecoder)
        {
            _keyFilter = filter;
            _tempKeyRepository = repository;
            _logger = logger;
            _riskCalculator = riskCalculator;
            _epochConverter = epochConverter;
            _daysSinceOnsetOfSymptomsDecoder = daysSinceOnsetOfSymptomsDecoder;
        }

        public IList<TemporaryExposureKey> FilterAndSaveKeys(IList<TemporaryExposureKeyGatewayDto> keys)
        {
            StringBuilder logErrorPerBatchSB = new StringBuilder();
            IList<TemporaryExposureKey> mappedKeys;

            IList<string> validationErrors = new List<string>();
            try
            {
                mappedKeys = _keyFilter.MapKeys(keys);
            }
            catch (Exception e)
            {
                _logger.LogError("|SmitteStop| StoreService: unable to parse and map response" + e);
                throw;
            }

            _logger.LogInformation($"Mapped  {mappedKeys?.Count} keys from {keys?.Count}");
            IList<TemporaryExposureKey> acceptedKeys = null;
            int acceptedKeysCount = 0;
            if (mappedKeys.Any())
            {
                foreach (var key in mappedKeys)
                {
                    CalulateDaysAndRisk(key);

                }
                _logger.LogInformation($"Starting key validation.");
                acceptedKeys = _keyFilter.ValidateKeys(mappedKeys, out validationErrors);
                _logger.LogInformation($"Keys validated with {validationErrors?.Count} error.");
                _logger.LogInformation($"Saving...");
                _tempKeyRepository.AddUniqueTemporaryExposureKeys(acceptedKeys).Wait();
                _logger.LogInformation($"{acceptedKeys?.Count} keys saved.");
                acceptedKeysCount = acceptedKeys.Count;
            }
            _logger.LogInformation($"Received {keys.Count} keys. Accepted and saved { acceptedKeysCount } of them.");

            foreach (string error in validationErrors)
            {
                logErrorPerBatchSB.Append(error + Environment.NewLine);
            }

            var errors = logErrorPerBatchSB.ToString();
            if (!string.IsNullOrEmpty(errors))
            {
                _logger.LogWarning($"Received {keys.Count} keys. Accepted and saved { acceptedKeysCount } of them. Validation Errors:{Environment.NewLine}{ errors }");
            }
            return acceptedKeys ?? new List<TemporaryExposureKey>();
        }

        private void CalulateDaysAndRisk(TemporaryExposureKey key)
        {
            if (key.DaysSinceOnsetOfSymptoms.HasValue)
            {
                var dsosWithOffset = key.DaysSinceOnsetOfSymptoms.Value;
                var result = _daysSinceOnsetOfSymptomsDecoder.Decode(dsosWithOffset);

                if (result.IsValid)
                {
                    if (result.SymptomStatus == SymptomStatus.Symptomatic)
                    {
                        switch (result.DateType)
                        {
                            case DateType.Unknown:
                                SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(key);
                                break;
                            case DateType.PreciseDate:
                                // <-14, 14> - normal format
                                key.DaysSinceOnsetOfSymptoms = result.DaysSinceOnsetOfSymptoms;
                                key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(key);
                                break;
                            case DateType.Range:
                                if (result.DaysSinceOnsetOfSymptoms > 0)
                                {
                                    // end of the interval
                                    key.DaysSinceOnsetOfSymptoms = result.DaysSinceOnsetOfSymptoms;
                                    key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(key);
                                }
                                else
                                {
                                    if (result.DaysSinceOnsetOfSymptoms.HasValue && Math.Abs(result.DaysSinceOnsetOfSymptoms.Value) >= result.IntervalDuration)
                                    {
                                        // begin of the interval
                                        key.DaysSinceOnsetOfSymptoms = result.DaysSinceOnsetOfSymptoms + result.IntervalDuration - 1;
                                        key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(key);

                                    }
                                    else
                                    {
                                        key.DaysSinceOnsetOfSymptoms = 0;
                                        key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(key);
                                        // inside of the interval
                                    }
                                }

                                break;
                            default:
                                throw new Exception("Missing implementation!");
                        }



                    }
                    else if (result.SymptomStatus == SymptomStatus.Asymptomatic)
                    {
                        SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(key);
                    }
                    else
                    {
                        // Unknown
                        SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(key);
                    }
                }
                else
                {
                    // unknown
                    SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(key);
                }
            }
            else
            {
                SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(key);
            }
        }

        private void SetRiskWhenDaysSinceOnsetOfSymptomsIsUnknown(TemporaryExposureKey key)
        {
            // DaysSinceOnsetOfSymptoms are unknown
            var keyCreationDate = _epochConverter.ConvertFromEpoch(key.RollingStartNumber);
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7).Date;
            if (sevenDaysAgo > keyCreationDate)
            {
                key.TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOWEST; // TODO!!! index! 
            }
            else
            {
                key.TransmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM; // TODO!!! Highest risk level - RiskLevel enum is not a risk level it is a index in the "TransmissionRiskScores" from config.
            }
            key.DaysSinceOnsetOfSymptoms = 0;
        }
    }
}
