using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var newKeyData = incomingKeys.Select(u => u.KeyData).Distinct().ToArray();
            var keysInDb = await _temporaryExposureKeyRepository.GetKeysThatAlreadyExistsInDbAsync(newKeyData);
            var keysNotInDb = incomingKeys.Where(u => keysInDb.All(x => !x.SequenceEqual(u.KeyData))).ToList();
            return keysNotInDb;
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
                key.ReportType = ReportType.CONFIRMED_TEST;
            }

            var visitedCountries = parameters.visitedCountries.FindAll(countryCode => countryCode.ToLower() != origin.Code.ToLower());
            await _temporaryExposureKeyRepository.AddTemporaryExposureKeysAsync(newTemporaryExposureKeys);
            await CreateKeyCountryRelationships(visitedCountries, newTemporaryExposureKeys);
        }
    }
}
