using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface ICovidStatisticsDataExtractingService
    {
        public App.SmitteStop.Domain.Db.CovidStatistics ProcessData(CovidStatisticsCsvContent csvContent);
    }
}