using Microsoft.Extensions.Logging;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsRetrieveService : ICovidStatisticsRetrieveService
    {
        private readonly ICovidStatisticsFilePackageBuilder _covidStatisticsFilePackageBuilder;
        private readonly ICovidStatisticsDataExtractingService _covidStatisticsDataExtractingService;
        private readonly ICovidStatisticsCsvParser _covidStatisticsCsvParser;
        private readonly ILogger<CovidStatisticsRetrieveService> _logger;

        public CovidStatisticsRetrieveService(ILogger<CovidStatisticsRetrieveService> logger,
            ICovidStatisticsFilePackageBuilder covidStatisticsFilePackageBuilder,
            ICovidStatisticsDataExtractingService covidStatisticsDataExtractingService,
            ICovidStatisticsCsvParser covidStatisticsCsvParser)
        {
            _logger = logger;
            _covidStatisticsDataExtractingService = covidStatisticsDataExtractingService;
            _covidStatisticsCsvParser = covidStatisticsCsvParser;
            _covidStatisticsFilePackageBuilder = covidStatisticsFilePackageBuilder;
        }

        public void GetCovidStatistics()
        {
            using var covidStatisticsSourceFileStreamsPackage = _covidStatisticsFilePackageBuilder.GetCovidStatisticsFilesPackage();
            var covidStatisticsFilesContent = _covidStatisticsCsvParser.ParsePackage(covidStatisticsSourceFileStreamsPackage);
            _covidStatisticsDataExtractingService.ProcessData(covidStatisticsFilesContent);
        }
    }
}