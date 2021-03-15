using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface IFetchCovidStatisticsService
    {
        Stream FetchTestNumbersFromDate(DateTime date);
        Stream FetchHospitalNumbersFromDate(DateTime date);
        Stream FetchVaccineNumbersFromDate(DateTime date);

    }
}
