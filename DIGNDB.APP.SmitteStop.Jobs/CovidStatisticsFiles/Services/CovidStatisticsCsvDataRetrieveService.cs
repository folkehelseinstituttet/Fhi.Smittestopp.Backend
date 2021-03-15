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
        public double GetMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector)
        {
            var newestRecord = GetRecord(fileContent, _dateTimeResolver.GetDateXDaysAgo(NewestRecordAgeInDays));
            return fieldSelector(newestRecord);
        }

        public double GetSumOfEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector)
        {
            double total = 0;
            foreach (var record in fileContent)
            {
                total += fieldSelector(record);
            }

            return total;
        }

        public double GetDifferenceBetweenMostRecentEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector)
        {
            var newestRecord = GetRecord(fileContent, _dateTimeResolver.GetDateXDaysAgo(NewestRecordAgeInDays));
            var secondNewestRecord = GetRecord(fileContent, _dateTimeResolver.GetDateXDaysAgo(NewestRecordAgeInDays + 1));
            return fieldSelector(newestRecord) - fieldSelector(secondNewestRecord);
        }

        private static CovidStatisticCsvFileContent GetRecord(IEnumerable<CovidStatisticCsvFileContent> fileContent, DateTime dateTime)
        {
            try
            {
                return fileContent.SingleOrDefault(x => x.Date == dateTime) ??
                               throw new CovidStatisticsCsvDataPartiallyMissingException(dateTime);
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