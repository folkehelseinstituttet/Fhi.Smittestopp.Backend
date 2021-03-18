using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using System;
using System.Collections.Generic;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface ICovidStatisticsCsvDataRetrieveService
    {
        int GetFromMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, int> fieldSelector);
        int GetSumFromEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, int> fieldSelector);
    }
}