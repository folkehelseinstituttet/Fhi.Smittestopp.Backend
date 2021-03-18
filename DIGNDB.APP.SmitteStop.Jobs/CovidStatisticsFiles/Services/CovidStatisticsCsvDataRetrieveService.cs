using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsCsvDataRetrieveService : ICovidStatisticsCsvDataRetrieveService
    {
        private const int NewestRecordAgeInDays = 1;
        private readonly IDateTimeResolver _dateTimeResolver;

        public CovidStatisticsCsvDataRetrieveService(IDateTimeResolver dateTimeResolver)
        {
            _dateTimeResolver = dateTimeResolver;
        }

        public int GetEntryByDate(IEnumerable<CovidStatisticCsvFileContent> fileContent, DateTime dateTime,
            Func<CovidStatisticCsvFileContent, int> fieldSelector)
        {
            var newestRecord = GetRecord(fileContent, dateTime);
            return fieldSelector(newestRecord);
        }

        public int GetFromMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent,
            Func<CovidStatisticCsvFileContent, int> fieldSelector)
        {
            var newestRecord = GetRecord(fileContent, _dateTimeResolver.GetDateXDaysAgo(NewestRecordAgeInDays));
            return fieldSelector(newestRecord);
        }

        public int GetSumFromEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, int> fieldSelector)
        {
            var total = 0;
            foreach (var record in fileContent)
            {
                total += fieldSelector(record);
            }

            return total;
        }

        private static CovidStatisticCsvFileContent GetRecord(IEnumerable<CovidStatisticCsvFileContent> fileContent,
            DateTime dateTime)
        {
            try
            {
                var retVal = fileContent.SingleOrDefault(x => x.Date == dateTime) ??
                             throw new CovidStatisticsCsvDataPartiallyMissingException(dateTime);
                return retVal;
            }
            catch (InvalidOperationException e)
            {
                throw new CovidStatisticsCsvDataNotUniqueException(dateTime, e);
            }
            catch (ArgumentException)
            {
                throw new CovidStatisticsCsvDataPartiallyMissingException(dateTime);
            }
        }
    }
}