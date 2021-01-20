using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<IEnumerable<Country>> GetVisibleCountries();
        Task<IEnumerable<Country>> GetVisibleCountries(string countryCode);
        Task<HashSet<long>> GetWhitelistHashSet();
    }
}