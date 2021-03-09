using CsvHelper.Configuration;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Mappings
{
    public class CovidStatisticsClassMapBase<T> : ClassMap<T> where T : CovidStatisticCsvFileContent
    {
        private const string DateName = "date";
        public CovidStatisticsClassMapBase()
        {
            Map(m => m.Date).Name(DateName);
        }
    }
}