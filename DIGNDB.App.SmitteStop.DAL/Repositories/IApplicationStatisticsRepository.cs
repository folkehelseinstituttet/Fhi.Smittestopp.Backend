using DIGNDB.App.SmitteStop.Domain.Db;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface IApplicationStatisticsRepository
    {
        public Task<ApplicationStatistics> GetNewestEntryAsync();

        public void UpdateEntry(ApplicationStatistics setting);
    }
}
