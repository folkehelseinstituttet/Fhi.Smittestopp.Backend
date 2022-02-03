using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class AddTemporaryExposureKeyService : IAddTemporaryExposureKeyService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IGenericRepository<TemporaryExposureKeyCountry> _temporaryExposureKeyCountryRepository;
        private readonly ITemporaryExposureKeyRepository _temporaryExposureKeyRepository;
        private readonly IExposureKeyMapper _exposureKeyMapper;
        private readonly ILogger<AddTemporaryExposureKeyService> _logger;

        public AddTemporaryExposureKeyService(ICountryRepository countryRepository, IGenericRepository<TemporaryExposureKeyCountry> temporaryExposureKeyCountryRepository,
            IExposureKeyMapper exposureKeyMapper, ITemporaryExposureKeyRepository temporaryExposureKeyRepository, ILogger<AddTemporaryExposureKeyService> logger)
        {
            _countryRepository = countryRepository;
            _temporaryExposureKeyCountryRepository = temporaryExposureKeyCountryRepository;
            _temporaryExposureKeyRepository = temporaryExposureKeyRepository;
            _exposureKeyMapper = exposureKeyMapper;
            _logger = logger;
        }

        public async Task CreateKeysInDatabase(TemporaryExposureKeyBatchDto parameters, KeySource apiVersion)
        {
            var newTemporaryExposureKeys = await GetFilteredKeysEntitiesFromDTO(parameters);
            if (newTemporaryExposureKeys.Any())
            {
                await CreateNewKeysInDatabase(parameters, newTemporaryExposureKeys, apiVersion);
            }
        }

        public async Task<IList<TemporaryExposureKey>> GetFilteredKeysEntitiesFromDTO(
            TemporaryExposureKeyBatchDto parameters)
        {
            var incomingKeys = _exposureKeyMapper.FromDtoToEntity(parameters);
            incomingKeys = await FilterDuplicateKeysAsync(incomingKeys);
            return incomingKeys;
        }

        public async Task<List<TemporaryExposureKey>> FilterDuplicateKeysAsync(IList<TemporaryExposureKey> incomingKeys)
        {
            var distinctIncomingKeys = FilterIncomingKeysForDuplicates(incomingKeys);

            var newKeyData = distinctIncomingKeys.Select(u => u.KeyData).Distinct().ToArray();
            var keysInDb = await _temporaryExposureKeyRepository.GetKeysThatAlreadyExistsInDbAsync(newKeyData);
            var keysNotInDb = distinctIncomingKeys.Where(u => keysInDb.All(x => !x.SequenceEqual(u.KeyData))).ToList();
            return keysNotInDb;
        }

        private IList<TemporaryExposureKey> FilterIncomingKeysForDuplicates(IList<TemporaryExposureKey> incomingKeys)
        {
            _logger.LogInformation($"No. of incoming keys before filtering {incomingKeys.Count}");

            var retVal = new List<TemporaryExposureKey>();

            foreach (var temporaryExposureKey in incomingKeys)
            {
                var contains = retVal.Any(x => x.KeyData.SequenceEqual(temporaryExposureKey.KeyData));
                if (contains)
                {
                    continue;
                }

                retVal.Add(temporaryExposureKey);
            }

            _logger.LogInformation($"No. of keys returned {retVal.Count}");

            return retVal;
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

        private async Task CreateNewKeysInDatabase(TemporaryExposureKeyBatchDto parameters, IList<TemporaryExposureKey> newTemporaryExposureKeys, KeySource apiVersion)
        {
            var origin = _countryRepository.FindByIsoCode(parameters.regions[0]);
            foreach (var key in newTemporaryExposureKeys)
            {
                key.Origin = origin;
                key.KeySource = apiVersion;
                key.ReportType = parameters.ReportType ?? ReportType.CONFIRMED_TEST ;
            }

            var visitedCountries = parameters.visitedCountries.FindAll(countryCode => countryCode.ToLower() != origin.Code.ToLower());
            await _temporaryExposureKeyRepository.AddTemporaryExposureKeysAsync(newTemporaryExposureKeys);
            await CreateKeyCountryRelationships(visitedCountries, newTemporaryExposureKeys);
        }
    }
}
