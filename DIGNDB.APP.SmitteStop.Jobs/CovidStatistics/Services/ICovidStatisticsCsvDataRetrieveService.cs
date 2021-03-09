using System;
using System.Collections.Generic;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface ICovidStatisticsCsvDataRetrieveService
    {
        double GetMostRecentEntry(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
        double GetSumOfEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
        double GetDifferenceBetweenMostRecentEntries(IEnumerable<CovidStatisticCsvFileContent> fileContent, Func<CovidStatisticCsvFileContent, double> fieldSelector);
    }
}