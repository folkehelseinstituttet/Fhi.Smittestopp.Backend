using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class ApplicationStatisticsRepository : GenericRepository<ApplicationStatistics>, IApplicationStatisticsRepository
    {
        public ApplicationStatisticsRepository(DigNDB_SmittestopContext context) : base(context) { }
        public async Task<ApplicationStatistics> GetNewestEntryAsync()
        {
            return await _context.ApplicationStatistics.Select(x => x).OrderByDescending(x => x.EntryDate).FirstOrDefaultAsync();
        }
    }
}
