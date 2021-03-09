using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class CovidStatisticsRepository : GenericRepository<CovidStatistics>, ICovidStatisticsRepository
    {
        public CovidStatisticsRepository(DigNDB_SmittestopContext context) : base(context)
        {
        }

        public CovidStatistics GetEntryByDate(DateTime date)
        {
            return Get(filter: c => c.Date.Date.CompareTo(date.Date) == 0).SingleOrDefault();
        }

        public async Task<CovidStatistics> GetEntryByDateAsync(DateTime date)
        {
            return (await GetAsync(filter: c => c.Date.Date == date.Date)).SingleOrDefault();
        }

        public void CreateEntry(CovidStatistics entry)
        {
            Insert(entry);
            Save();
        }

        public void RemoveEntriesOlderThan(DateTime date)
        {
            var allRecords = Get(filter: x => x.Date.Date <= date.Date);
            foreach (var record in allRecords)
            {
                Delete(record);
            }
            Save();
        }

        public async Task<CovidStatistics> GetNewestEntryAsync()
        {
            return (await GetAsync(orderBy: x => x.OrderByDescending(x => x.Date))).First();
        }
    }
}
