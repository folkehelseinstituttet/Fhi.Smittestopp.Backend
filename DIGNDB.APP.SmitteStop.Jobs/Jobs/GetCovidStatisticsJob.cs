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
            var currentEntryDate =
                _covidStatisticsRepository.GetNewestEntry()?.Date ?? DateTime.UtcNow.AddDays(-1);
            while (currentEntryDate.Date < DateTime.UtcNow.Date)
            {
                currentEntryDate = currentEntryDate.AddDays(1);
                if (!(currentEntryDate.DayOfWeek == DayOfWeek.Saturday ||
                      currentEntryDate.DayOfWeek == DayOfWeek.Sunday))
                {
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
        }

        private void HandleDataMissing()
        {
            if (_dateTimeResolver.GetDateTimeNow().Hour < _config.MakeAlertIfDataIsMissingAfterHour)
            {
                _logger.LogInformation(CovidStatisticsFilleMissingOnServerException.DataMissingInfoMessage);
            }
            else
            {
                _logger.LogError(CovidStatisticsFilleMissingOnServerException.DataMissingErrorMessage);
                throw new CovidStatisticsFilleMissingOnServerException();
            }
        }
    }
}
