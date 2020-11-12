using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class AddTemporaryExposureKeyService : IAddTemporaryExposureKeyService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IGenericRepository<TemporaryExposureKeyCountry> _temporaryExposureKeyCountryRepository;
        private readonly ITemporaryExposureKeyRepository _temporaryExposureKeyRepository;
        private readonly IExposureKeyMapper _exposureKeyMapper;
        private readonly AppSettingsConfig _appSettingsConfig;

        public AddTemporaryExposureKeyService(ICountryRepository countryRepository, IGenericRepository<TemporaryExposureKeyCountry> temporaryExposureKeyCountryRepository,
            IExposureKeyMapper exposureKeyMapper, ITemporaryExposureKeyRepository temporaryExposureKeyRepository, AppSettingsConfig appSettingsConfig)
        {
            _countryRepository = countryRepository;
            _temporaryExposureKeyCountryRepository = temporaryExposureKeyCountryRepository;
            _temporaryExposureKeyRepository = temporaryExposureKeyRepository;
            _exposureKeyMapper = exposureKeyMapper;
            _appSettingsConfig = appSettingsConfig;
        }

        public async Task CreateKeysInDatabase(TemporaryExposureKeyBatchDto parameters)
        {
            var newTemporaryExposureKeys = await GetFilteredKeysEntitiesFromDTO(parameters);
            if (newTemporaryExposureKeys.Any())
            {
                await CreateNewKeysInDatabase(parameters, newTemporaryExposureKeys);
            }
        }

        public async Task<IList<TemporaryExposureKey>> GetFilteredKeysEntitiesFromDTO(TemporaryExposureKeyBatchDto parameters)
        {
            var incomingKeys = _exposureKeyMapper.FromDtoToEntity(parameters);
            incomingKeys = await FilterDuplicateIncommingKeys(incomingKeys);
            return incomingKeys;
        }

        private async Task<List<TemporaryExposureKey>> FilterDuplicateIncommingKeys(List<TemporaryExposureKey> incomingKeys)
        {
            int numberOfRecordsToSkip = 0;
            int batchSize = _appSettingsConfig.MaxKeysPerFile;
            long lowestRollingStartNumber = GetLowestRollingStartNumber(incomingKeys);
            var existingKeysBatch = await _temporaryExposureKeyRepository.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(lowestRollingStartNumber, numberOfRecordsToSkip, batchSize);
            while (existingKeysBatch.Count != 0)
            {
                incomingKeys = _exposureKeyMapper.FilterDuplicateKeys(incomingKeys, existingKeysBatch).ToList();
                numberOfRecordsToSkip += existingKeysBatch.Count;
                existingKeysBatch = await _temporaryExposureKeyRepository.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(lowestRollingStartNumber, numberOfRecordsToSkip, batchSize);
            }
            return incomingKeys;
        }

        private long GetLowestRollingStartNumber(List<TemporaryExposureKey> incomingKeys)
        {
            long lowestRollingStartNumber = int.MaxValue;
            foreach (var key in incomingKeys)
            {
                if (key.RollingStartNumber<lowestRollingStartNumber)
                {
                    lowestRollingStartNumber = key.RollingStartNumber;
                }
            }
            if (lowestRollingStartNumber == int.MaxValue)
            {
                lowestRollingStartNumber = 0;
            }
            return lowestRollingStartNumber;
        }

        private async Task CreateKeyCountryRelationships(List<string> visitedCountries, IList<TemporaryExposureKey> newTemporaryExposureKeys)
        {
            var visitedCountriesEntities = _countryRepository.FindByIsoCodes(visitedCountries);

            foreach (var region in visitedCountriesEntities)
            {
                foreach (var key in newTemporaryExposureKeys)
                {
                    var keyCountry = new TemporaryExposureKeyCountry()
                    {
                        TemporaryExposureKey = key,
                        CountryId = region.Id
                    };
                    _temporaryExposureKeyCountryRepository.Insert(keyCountry);
                }
            }
            await _temporaryExposureKeyCountryRepository.SaveAsync();
        }

        private async Task CreateNewKeysInDatabase(TemporaryExposureKeyBatchDto parameters, IList<TemporaryExposureKey> newTemporaryExposureKeys)
        {
            var origin = _countryRepository.FindByIsoCode(parameters.regions[0]);
            foreach (var key in newTemporaryExposureKeys)
            {
                key.Origin = origin;
                key.KeySource = KeySource.SmitteStopApiVersion2;
                key.ReportType = ReportType.CONFIRMED_TEST;
            }

            var visitedCountries = parameters.visitedCountries.FindAll(countryCode => countryCode.ToLower() != origin.Code.ToLower());

            await _temporaryExposureKeyRepository.AddTemporaryExposureKeys(newTemporaryExposureKeys);
            await CreateKeyCountryRelationships(visitedCountries, newTemporaryExposureKeys);
        }
    }
}
