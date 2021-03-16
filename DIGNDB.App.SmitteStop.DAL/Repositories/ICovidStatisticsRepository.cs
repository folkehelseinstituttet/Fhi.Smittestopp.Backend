using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface ICovidStatisticsRepository
    {
        public Task<CovidStatistics> GetEntryByDateAsync(DateTime date);
        CovidStatistics GetEntryByDate(DateTime date);
        void CreateEntry(CovidStatistics entry);
        void Delete(CovidStatistics existingCovidStatistics);
        Task<CovidStatistics> GetNewestEntryAsync();
        CovidStatistics GetNewestEntry();
    }
}
