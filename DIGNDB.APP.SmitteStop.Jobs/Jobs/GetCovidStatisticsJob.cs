using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class GetCovidStatisticsJob
    {
        private readonly ILogger<CovidStatisticsBuilder> _logger;
        private readonly IDateTimeResolver _dateTimeResolver;
        private readonly ICovidStatisticsRepository _covidStatisticsRepository;
        private readonly ICovidStatisticsRetrieveService _covidStatisticsRetrieveService;
        private readonly GetCovidStatisticsJobConfig _config;

        public GetCovidStatisticsJob(ILogger<CovidStatisticsBuilder> logger,
            IDateTimeResolver dateTimeResolver,
            ICovidStatisticsRepository covidStatisticsRepository,
            ICovidStatisticsRetrieveService covidStatisticsRetrieveService,
            GetCovidStatisticsJobConfig config)
        {
            _config = config;
            _covidStatisticsRetrieveService = covidStatisticsRetrieveService;
            _covidStatisticsRepository = covidStatisticsRepository;
            _dateTimeResolver = dateTimeResolver;
            _logger = logger;
        }

        public void ObtainCovidStatistics()
        {
            var newestEntry = _covidStatisticsRepository.GetNewestEntry();
            var currentEntryDate = newestEntry?.EntryDate.Date ?? DateTime.UtcNow.Date.AddDays(-1);

            while (currentEntryDate < DateTime.UtcNow.Date)
            {
                currentEntryDate = currentEntryDate.AddDays(1);
                if (IsWeekendDay(currentEntryDate))
                {
                    continue;
                }

                _dateTimeResolver.SetDateTime(currentEntryDate);
                try
                {
                    _covidStatisticsRetrieveService.GetCovidStatistics();
                }
                catch (FileNotFoundException)
                {
                    HandleDataMissing();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Unexpected error while calculating covid statistics: {e}");
                    throw;
                }
            }
        }

        private static bool IsWeekendDay(DateTime currentEntryDate)
        {
            var dayOfWeek = currentEntryDate.DayOfWeek;
            var weekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
            return weekend;
        }

        private void HandleDataMissing()
        {
            if (_dateTimeResolver.GetDateTime().Hour < _config.MakeAlertIfDataIsMissingAfterHour)
            {
                _logger.LogInformation(CovidStatisticsFileMissingOnServerException.DataMissingInfoMessage);
            }
            else
            {
                _logger.LogError(CovidStatisticsFileMissingOnServerException.DataMissingErrorMessage);
                throw new CovidStatisticsFileMissingOnServerException();
            }
        }
    }
}
