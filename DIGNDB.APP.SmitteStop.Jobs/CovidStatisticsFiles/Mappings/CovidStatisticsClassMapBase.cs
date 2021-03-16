using CsvHelper.Configuration;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
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