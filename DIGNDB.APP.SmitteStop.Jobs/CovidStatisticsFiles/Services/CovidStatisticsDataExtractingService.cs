using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsDataExtractingService : ICovidStatisticsDataExtractingService
    {
        private readonly ICovidStatisticsRepository _covidStatisticsRepository;
        private readonly ICovidStatisticsBuilder _covidStatisticsBuilder;

        public CovidStatisticsDataExtractingService(ICovidStatisticsRepository covidStatisticsRepository, ICovidStatisticsBuilder covidStatisticsBuilder)
        {
            _covidStatisticsBuilder = covidStatisticsBuilder;
            _covidStatisticsRepository = covidStatisticsRepository;
        }
        public App.SmitteStop.Domain.Db.CovidStatistics ProcessData(CovidStatisticsCsvContent csvContent)
        {
            var statistics = _covidStatisticsBuilder.BuildStatistics(csvContent);
            _covidStatisticsRepository.CreateEntry(statistics);
            return statistics;
        }
    }
}