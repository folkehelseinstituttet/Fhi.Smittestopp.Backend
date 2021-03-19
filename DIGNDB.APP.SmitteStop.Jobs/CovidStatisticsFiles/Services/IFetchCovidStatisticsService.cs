using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface IFetchCovidStatisticsService
    {
        Stream FetchTestNumbersFromDate(DateTime date);
        Stream FetchHospitalNumbersFromDate(DateTime date);
        Stream FetchVaccineNumbersFromDate(DateTime date);
        Stream FetchConfirmedCasesTodayNumbersFromDate(DateTime date);
        Stream FetchConfirmedCasesTotalNumbersFromDate(DateTime date);
    }
}
