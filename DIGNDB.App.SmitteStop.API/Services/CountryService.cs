using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryService> _logger;

        public CountryService(ICountryRepository countryRepository, ILogger<CountryService> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Country>> GetVisibleCountries()
        {
            return await _countryRepository.GetVisibleAsync();
        }

        public async Task<IEnumerable<Country>> GetVisibleCountries(string countryCode)
        {
            var countries = await _countryRepository.GetVisibleAsync();
            var countriesWithRightTranslations = FilterOutIrrelevantTranslations(countries, countryCode);
            countriesWithRightTranslations = countriesWithRightTranslations.OrderBy(x => x.EntityTranslations.Single().Value);
            return countriesWithRightTranslations;
        }

        private IEnumerable<Country> FilterOutIrrelevantTranslations(IEnumerable<Country> countries, string countryCode)
        {
            var countriesWithRightTranslations = new List<Country>();
            foreach (var country in countries)
            {
                var translations = country.EntityTranslations;
                if (!translations.Any())
                {
                    _logger.LogError($"No translation available for country {country.Code}");
                    continue;
                }

                var translation = translations.Single(x => x.LanguageCountry.Code == countryCode);
                var translationValue = translation.Value;


                countriesWithRightTranslations.Add(new Country()
                {
                    Code = country.Code,
                    EntityTranslations = new List<Translation>()
                    {
                        new Translation
                        {
                            Value = translationValue
                        }
                    }
                });
            }

            return countriesWithRightTranslations;
        }

        public async Task<HashSet<long>> GetWhitelistHashSet()
        {
            var whitelistCountries = await _countryRepository.GetAllCountriesWithGatewayPullingEnabled();

            var whitelistHashSet = new HashSet<long>();
            foreach (var country in whitelistCountries)
                whitelistHashSet.Add(country.Id);

            return whitelistHashSet;
        }
    }
}