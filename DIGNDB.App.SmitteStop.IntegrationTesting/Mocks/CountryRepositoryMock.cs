using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    class CountryRepositoryMock : ICountryRepository
    {
        public Task<IEnumerable<Country>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Country>> GetVisibleAsync()
        {
            throw new NotImplementedException();
        }

        public Country FindByIsoCode(string isoCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> FindByIsoCodes(IList<string> countryCodes)
        {
            throw new NotImplementedException();
        }

        public Country GetApiOriginCountry()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Country>> GetAllCountriesWithGatewayPullingEnabled()
        {
            throw new NotImplementedException();
        }
    }
}
