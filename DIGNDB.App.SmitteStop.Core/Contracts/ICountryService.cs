using System.Collections.Generic;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<IEnumerable<Country>> GetVisibleCountries();
        Task<HashSet<long>> GetWhitelistHashSet();
    }
}