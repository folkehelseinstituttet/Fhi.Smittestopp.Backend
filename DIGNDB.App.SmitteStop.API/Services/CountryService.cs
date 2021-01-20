using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
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
                countriesWithRightTranslations.Add(new Country()
                {
                    Code = country.Code,
                    EntityTranslations = new List<Translation>()
                    {
                        new Translation()
                        {
                            Value = country.EntityTranslations.Single(x=>x.LanguageCountry.Code==countryCode).Value
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