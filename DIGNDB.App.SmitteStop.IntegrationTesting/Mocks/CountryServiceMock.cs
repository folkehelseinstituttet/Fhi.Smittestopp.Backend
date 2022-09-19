using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    class CountryServiceMock : ICountryService
    {
        private List<Country> _exampleCountries = new List<Country>
        {
            new Country
            {
                Code = "PL",
                Id = 1,
                PullingFromGatewayEnabled = true
            },
            new Country
            {
                Code = "SE",
                Id = 2,
                PullingFromGatewayEnabled = true
            }
        };
        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            return _exampleCountries;
        }

        public async Task<IEnumerable<Country>> GetVisibleCountries()
        {
            return   _exampleCountries;
        }

        public async Task<IEnumerable<Country>> GetVisibleCountries(string countryCode)
        {
            return _exampleCountries;
        }

        public async Task<HashSet<long>> GetWhitelistHashSet()
        {
            throw new NotImplementedException();
        }
    }
}
