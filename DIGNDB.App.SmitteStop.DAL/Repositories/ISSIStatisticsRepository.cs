using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface ISSIStatisticsRepository
    {
        public Task<SSIStatistics> GetEntryByDateAsync(DateTime date);
        SSIStatistics GetEntryByDate(DateTime date);
        void CreateEntry(SSIStatistics entry);
        void RemoveEntriesOlderThan(DateTime date);
        void Delete(SSIStatistics existingSsiStatistics);
        Task<SSIStatistics> GetNewestEntryAsync();
    }
}
