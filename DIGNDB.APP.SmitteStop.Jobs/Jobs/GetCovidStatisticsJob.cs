using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Utils;
using Microsoft.Extensions.Logging;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class GetCovidStatisticsJob
    {
        private readonly ILogger<CovidStatisticsBuilder> _logger;
        private readonly IDateTimeResolver _dateTimeResolver;
        private readonly ICovidStatisticsRepository _covidStatisticsRepository;
        private readonly ICovidStatisticsRetrieveService _covidStatisticsRetrieveService;

        public GetCovidStatisticsJob(ILogger<CovidStatisticsBuilder> logger,
            IDateTimeResolver dateTimeResolver,
            ICovidStatisticsRepository covidStatisticsRepository,
            ICovidStatisticsRetrieveService covidStatisticsRetrieveService)
        {
            _covidStatisticsRetrieveService = covidStatisticsRetrieveService;
            _covidStatisticsRepository = covidStatisticsRepository;
            _dateTimeResolver = dateTimeResolver;
            _logger = logger;
        }

        public void GetCovidStatisticsIfEntryDoesNotExists()
        {
            SetDateTimeToNow();
            if (StatisticsRecordAlreadyExists())
            {
                _logger.LogInformation("Statistics entry already exists. Aborting.");
            }
            else
            {
                _covidStatisticsRetrieveService.GetCovidStatistics();
            }
        }

        private bool StatisticsRecordAlreadyExists()
        {
            return !(_covidStatisticsRepository.GetEntryByDate(_dateTimeResolver.GetDateToday()) is null);
        }

        private void SetDateTimeToNow()
        {
            _dateTimeResolver.SetDateTimeToUtcNow();
        }
    }
}
