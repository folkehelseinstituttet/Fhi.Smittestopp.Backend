using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface ICovidStatisticsDataExtractingService
    {
        public App.SmitteStop.Domain.Db.CovidStatistics ProcessData(CovidStatisticsCsvContent csvContent);
    }
}