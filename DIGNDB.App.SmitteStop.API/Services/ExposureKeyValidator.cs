using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class ExposureKeyValidator : IExposureKeyValidator
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<ExposureKeyValidator> _logger;

        public ExposureKeyValidator(ICountryRepository countryRepository, ILogger<ExposureKeyValidator> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public void ValidateParameterAndThrowIfIncorrect(TemporaryExposureKeyBatchDto parameter, KeyValidationConfiguration configuration)
        {
            ValidateKeyCount(parameter, configuration);
            ValidateKeyDuplication(parameter);
            ValidateKeyDataLength(parameter);
            ValidateRollingStartDate(parameter, configuration);
            ValidateRollingDurationSpan(parameter);
            ValidateRollingStartDateIntervals(parameter);
            ValidateRegions(parameter);
            ValidateVisitedCountries(parameter);
            ValidatePackageNames(parameter, configuration);
        }

        private void ValidateRollingStartDateIntervals(TemporaryExposureKeyBatchDto parameter)
        {
            //Any ENIntervalNumber values from the same user are not unique
            var rollingStartGroupsHavingMoreThan1Key =
                parameter.keys.GroupBy(k => k.rollingStart).Where(group => group.Count() > 1).ToList();
            if (!rollingStartGroupsHavingMoreThan1Key.Any()) return;

            var rollingStartDates = rollingStartGroupsHavingMoreThan1Key.Select(group => @group.Key);
            var errorMessage =
                $"Incorrect intervals. Dates having more than one key: {string.Join(", ", rollingStartDates)}";

            throw new ArgumentException(errorMessage);
        }

        private void ValidateKeyCount(TemporaryExposureKeyBatchDto parameter, KeyValidationConfiguration configuration)
        {
            //The period of time covered by the data file exceeds 14 days
            if (parameter.keys.Count <= configuration.OutdatedKeysDayOffset) return;

            var errorMessage =
                $"Incorrect key count. Given key count: {parameter.keys.Count}. Expected: {configuration.OutdatedKeysDayOffset}";
            throw new ArgumentException(errorMessage);
        }

        private void ValidateKeyDuplication(TemporaryExposureKeyBatchDto parameter)
        {
            var duplicatedKeys = GetDuplicatedKeys(parameter.keys.Select(k => k.key).ToList());
            if (!duplicatedKeys.Any()) return;

            var stringOfDuplicatedKeys = string.Join(", ",
                duplicatedKeys.Select(key => BitConverter.ToString(key).Replace("-", "")));
            var errorMessage =
                $"Duplicate key values. Duplicated keys: {stringOfDuplicatedKeys}";

            throw new ArgumentException(errorMessage);
        }

        private void ValidateKeyDataLength(TemporaryExposureKeyBatchDto parameter)
        {
            if (parameter.keys.Any(x => x.key.Length != TemporaryExposureKeyDto.KeyLength))
            {
                parameter.keys = GetKeysWithValidSize(parameter.keys, _logger);
            }
        }

        private static void ValidateRollingDurationSpan(TemporaryExposureKeyBatchDto parameter)
        {
            //The TEKRollingPeriod value is not 144
            var minimumValidSpan = TimeSpan.FromMinutes(10);
            var maximumValidSpan = TemporaryExposureKeyDto.OneDayTimeSpan;

            var incorrectRollingDurationSpans = parameter.keys.Where(k =>
                k.rollingDurationSpan > maximumValidSpan ||
                k.rollingDurationSpan < minimumValidSpan).ToList();

            if (!incorrectRollingDurationSpans.Any()) return;

            var spansString = string.Join(", ", incorrectRollingDurationSpans.Select(key => key.rollingDurationSpan));
            var errorMessage = $"Incorrect spans: {spansString}. Expecting span to be > {minimumValidSpan} and < {maximumValidSpan}";

            throw new ArgumentException(errorMessage);
        }

        private static void ValidateRollingStartDate(TemporaryExposureKeyBatchDto parameter,
            KeyValidationConfiguration configuration)
        {
            var outdatedKeysDate = DateTime.UtcNow.Date.AddDays(-configuration.OutdatedKeysDayOffset);
            var todaysDateUtcMidnight = DateTime.UtcNow.Date;

            var keysWithInvalidRollingStart =
                parameter.keys.Where(key => key.rollingStart < outdatedKeysDate || key.rollingStart > todaysDateUtcMidnight).ToList();
            if (!keysWithInvalidRollingStart.Any()) return;

            var invalidRollingStartDates = keysWithInvalidRollingStart.Select(key => key.rollingStart);
            var invalidRollingStartDatesString = string.Join(", ", invalidRollingStartDates);

            var errorMessage =
                $"Incorrect start date. Incorrect dates: {invalidRollingStartDatesString}. OutdatedKeysDayOffset from configuration: {configuration.OutdatedKeysDayOffset}";
            throw new ArgumentException(errorMessage);
        }

        private void ValidateRegions(TemporaryExposureKeyBatchDto parameter)
        {
            var originCountry = _countryRepository.GetApiOriginCountry();

            if (!parameter.keys.Any()) return;
            if (parameter.regions == null)
                throw new ArgumentException("Regions array is null.");

            var expectedRegionCode = originCountry.Code;
            var regions = parameter.regions;

            if (regions.Count == 1 && regions.Any(r => r.ToLower() == expectedRegionCode.ToLower())) return;

            var regionsString = string.Join(",", parameter.regions);
            throw new ArgumentException(
                $"Incorrect regions: {regionsString}. Expected single value: {expectedRegionCode}");
        }

        private void ValidatePackageNames(TemporaryExposureKeyBatchDto parameter, KeyValidationConfiguration configuration)
        {
            var packageName = parameter.appPackageName.ToLower();

            if (packageName == configuration.PackageNames.Apple ||
                packageName == configuration.PackageNames.Google) return;

            var errorMessage =
                $"Incorrect package name. Given: {packageName}.";
            errorMessage +=
                $"Expected for {nameof(configuration.PackageNames.Apple)}: {configuration.PackageNames.Apple}";
            errorMessage +=
                $"Expected for {nameof(configuration.PackageNames.Google)}: {configuration.PackageNames.Google}";

            throw new ArgumentException(errorMessage);
        }
        
        private void ValidateVisitedCountries(TemporaryExposureKeyBatchDto parameter)
        {
            if (parameter.visitedCountries == null) return;

            var notFoundVisitedCountries = new List<string>();
            var disabledVisitedCountries = new List<string>();
            foreach (var visitedCountry in parameter.visitedCountries)
            {
                var foundCountry = _countryRepository.FindByIsoCode(visitedCountry);

                if (foundCountry == null)
                    notFoundVisitedCountries.Add(visitedCountry);
                else if (!foundCountry.VisitedCountriesEnabled)
                    disabledVisitedCountries.Add(visitedCountry);
            }

            var errorMessage = string.Empty;
            if (notFoundVisitedCountries.Any())
            {
                errorMessage +=
                    $"ISO codes of countries not found in the database: {string.Join(", ", notFoundVisitedCountries)}. ";
            }
            if (disabledVisitedCountries.Any())
            {
                errorMessage +=
                    $"ISO codes of countries marked as disabled in the database: {string.Join(", ", disabledVisitedCountries)}. ";
            }

            if (notFoundVisitedCountries.Any() || disabledVisitedCountries.Any())
                throw new ArgumentException(errorMessage);
        }

        private List<TemporaryExposureKeyDto> GetKeysWithValidSize(IEnumerable<TemporaryExposureKeyDto> keys, ILogger logger)
        {
            var newNonValidExposureKeys = new List<TemporaryExposureKeyDto>();
            var newValidExposureKeys = new List<TemporaryExposureKeyDto>();

            foreach (var exposureKeyDto in keys)
            {
                if (exposureKeyDto.key.Length == TemporaryExposureKeyDto.KeyLength)
                    newValidExposureKeys.Add(exposureKeyDto);
                else
                    newNonValidExposureKeys.Add(exposureKeyDto);
            }

            // Logging keys with non valid size in a json format.
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(newNonValidExposureKeys.First().ToJson());
            newNonValidExposureKeys.ForEach(x => builder.Append("," + x.ToJson()));
            builder.Append("]");
            logger.LogError($"Detected {newNonValidExposureKeys.Count} keys with an invalid size: {builder}");

            return newValidExposureKeys;
        }

        private IList<byte[]> GetDuplicatedKeys(IList<byte[]> keys)
        {
            IList<byte[]> duplicatedKeys = new List<byte[]>();

            for (int i = 0; i < keys.Count - 1; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                    if (StructuralComparisons.StructuralEqualityComparer.Equals(keys[i], keys[j]))
                        duplicatedKeys.Add(keys[j]);
            }

            return duplicatedKeys;
        }
    }
}

