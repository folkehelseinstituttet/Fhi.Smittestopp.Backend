using DIGNDB.App.SmitteStop.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

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
