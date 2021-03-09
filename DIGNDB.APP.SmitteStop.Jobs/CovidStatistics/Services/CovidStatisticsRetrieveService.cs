using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
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
            try
            {
                using var covidStatisticsSourceFileStreamsPackage = _covidStatisticsFilePackageBuilder.GetCovidStatisticsFilesPackage();
                var covidStatisticsFilesContent = _covidStatisticsCsvParser.ParsePackage(covidStatisticsSourceFileStreamsPackage);
                _covidStatisticsDataExtractingService.ProcessData(covidStatisticsFilesContent);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(
                    $"There was an error while trying to obtain one of the files needed to build covid statistics. inner exception - {e}");
                throw;
            }
        }
    }
}