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
            var entries = _context.ApplicationStatistics
                .Select(x => x)
                .OrderByDescending(x => x.EntryDate)
                .AsNoTracking();

            if (entries == null)
            {
                return null;
            }

            var newest = await entries.FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return newest;
        }

        public void UpdateEntry(ApplicationStatistics entry)
        {
            _context.ApplicationStatistics.Update(entry);
            _context.SaveChanges();
        }
    }
}
