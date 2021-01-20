using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();

        Task<IEnumerable<Country>> GetVisibleAsync();

        Country FindByIsoCode(string isoCode);

        IEnumerable<Country> FindByIsoCodes(IList<string> countryCodes);

        Country GetApiOriginCountry();

        Task<IEnumerable<Country>> GetAllCountriesWithGatewayPullingEnabled();
    }
}
