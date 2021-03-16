using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using System;
using System.Collections.Generic;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface ICovidStatisticsCsvDataRetrieveService
    {
        double GetMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
        double GetSumOfEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
        double GetDifferenceBetweenMostRecentEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
    }
}