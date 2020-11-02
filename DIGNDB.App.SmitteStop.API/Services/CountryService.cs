using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Internal;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;

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

        public async Task<HashSet<long>> GetWhitelistHashSet()
        {
            var whitelistCountries = await _countryRepository.GetGetCountriesToPullFrom();

            var whitelistHashSet = new HashSet<long>();
            foreach (var country in whitelistCountries)
                whitelistHashSet.Add(country.Id);

            return whitelistHashSet;
        }
    }
}