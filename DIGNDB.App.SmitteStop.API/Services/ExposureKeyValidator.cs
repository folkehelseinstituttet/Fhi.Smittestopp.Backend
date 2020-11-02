using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DIGNDB.App.SmitteStop.API.Services
{

    public class ExposureKeyValidator : IExposureKeyValidator
    {
        private readonly ICountryRepository _countryRepository;

        public ExposureKeyValidator(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public void ValidateParameterAndThrowIfIncorrect(TemporaryExposureKeyBatchDto parameter, KeyValidationConfiguration configuration, ILogger logger)
        {
            var now = DateTime.UtcNow;
            var outdatedKeysDate = DateTime.UtcNow.Date.AddDays(-configuration.OutdatedKeysDayOffset);
            //The period of time covered by the data file exceeds 14 days
            if (parameter.keys.Count > configuration.OutdatedKeysDayOffset)
            {
                throw new ArgumentException("Incorrect key count.");
            }
            if (!ValidateForDuplicatedKeys(parameter.keys.Select(k => k.key).ToList()))
            {
                throw new ArgumentException("Duplicate key values.");
            }

            if (parameter.keys.Any(x => x.key.Length != TemporaryExposureKeyDto.KeyLength))
            {
                parameter.keys = GetKeysWithValidSize(parameter.keys, logger);
            }

            if (parameter.keys.Any(k => k.rollingStart < outdatedKeysDate || k.rollingStart > now))
            {
                throw new ArgumentException("Incorrect start date.");
            }

            //The TEKRollingPeriod value is not 144
            if (parameter.keys.Any(k => k.rollingDurationSpan > TemporaryExposureKeyDto.OneDayTimeSpan || k.rollingDurationSpan < TimeSpan.FromMinutes(10)))
            {
                throw new ArgumentException("Incorrect span.");
            }

            //Any ENIntervalNumber values from the same user are not unique
            if (parameter.keys.GroupBy(k => k.rollingStart).Any(group => group.Count() > 1))
            {
                throw new ArgumentException("Incorrect intervals.");
            }

            //There are any gaps in the ENIntervalNumber values for a user
            //Any keys in the file have overlapping time windows
            if (parameter.keys.Any())
            {
                if (parameter.regions == null || !configuration.Regions.Contains(parameter.regions.Single()?.ToLower()))
                {
                    throw new ArgumentException("Incorrect region.");
                }
            }
            if (parameter.visitedCountries != null)
            {
                foreach (var visitedCountry in parameter.visitedCountries)
                {
                    var countryDb = _countryRepository.FindByIsoCode(visitedCountry);
                    if (countryDb == null || countryDb.VisitedCountriesEnabled == false)
                    {
                        throw new ArgumentException("Incorrect visited countries.");
                    }
                }
            }

            if (configuration.PackageNames.Apple != parameter.appPackageName.ToLower() && configuration.PackageNames.Google != parameter.appPackageName.ToLower())
            {
                throw new ArgumentException("Incorrect package name.");
            }
        }

        public async Task ValidateDeviceVerificationPayload(TemporaryExposureKeyBatchDto parameter, IAppleService appleService, ILogger logger)
        {
            if (parameter.platform.ToLowerInvariant() == "ios")
            {
                AppleResponseDto apple = await appleService.ExecuteQueryBitsRequest(parameter.deviceVerificationPayload);
                if (apple.ResponseCode == HttpStatusCode.OK && !IsValidJson(apple.Content))
                {
                    apple = await appleService.ExecuteUpdateBitsRequest(parameter.deviceVerificationPayload);
                    if (apple.ResponseCode != HttpStatusCode.OK)
                        throw new ArgumentException("DeviceVerificationPayload invalid");
                    apple = await appleService.ExecuteQueryBitsRequest(parameter.deviceVerificationPayload);
                    if (apple.ResponseCode != HttpStatusCode.OK)
                        throw new ArgumentException("DeviceVerificationPayload invalid");
                }
                else if (apple.ResponseCode != HttpStatusCode.OK)
                    throw new ArgumentException("DeviceVerificationPayload invalid");
            }

            if (parameter.platform.ToLowerInvariant() == "android")
            {
                try
                {
                    var attestationStatement = GoogleTokenValidator.ParseAndVerify(parameter.deviceVerificationPayload, logger);
                    if (attestationStatement == null)
                    {
                        throw new Exception("Attestation statement empty.");
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("DeviceVerificationPayload invalid. " + e);
                }
            }
        }

        private List<TemporaryExposureKeyDto> GetKeysWithValidSize(List<TemporaryExposureKeyDto> keys, ILogger logger)
        {
            List<TemporaryExposureKeyDto> newNonValidExposureKeys = new List<TemporaryExposureKeyDto>();
            List<TemporaryExposureKeyDto> newValidExposureKeys = new List<TemporaryExposureKeyDto>();

            foreach (var exposureKeyDto in keys)
            {
                if (exposureKeyDto.key.Length == TemporaryExposureKeyDto.KeyLength)
                    newValidExposureKeys.Add(exposureKeyDto);
                else
                    newNonValidExposureKeys.Add(exposureKeyDto);
            }

            // Logging keys with non valid size in a json format.
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append(newNonValidExposureKeys.First().ToJson());
            newNonValidExposureKeys.ForEach(x => builder.Append("," + x.ToJson()));
            builder.Append("]");
            logger.LogError($"Detected {newNonValidExposureKeys.Count} keys with an invalid size: {builder}");

            return newValidExposureKeys;
        }

        private bool ValidateForDuplicatedKeys(IList<byte[]> keys)
        {
            for (int i = 0; i < keys.Count - 1; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                    if (StructuralComparisons.StructuralEqualityComparer.Equals(keys[i], keys[j]))
                        return false;
            }
            return true;
        }

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

