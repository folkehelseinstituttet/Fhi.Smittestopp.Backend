using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface ICovidStatisticsBuilder
    {
        App.SmitteStop.Domain.Db.CovidStatistics BuildStatistics(CovidStatisticsCsvContent inputData);
    }
}