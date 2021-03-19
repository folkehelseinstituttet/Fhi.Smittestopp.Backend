using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using System;
using System.Collections.Generic;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface ICovidStatisticsCsvDataRetrieveService
    {
        int GetEntryByDate(IEnumerable<CovidStatisticCsvFileContent> fileContent, DateTime dateTime, Func<CovidStatisticCsvFileContent, int> fieldSelector);
        int GetFromMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, int> fieldSelector);
        int GetSumFromEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, int> fieldSelector);
    }
}