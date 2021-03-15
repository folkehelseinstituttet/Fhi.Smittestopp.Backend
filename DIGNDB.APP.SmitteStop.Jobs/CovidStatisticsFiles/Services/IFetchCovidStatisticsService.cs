using System;
using System.IO;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface IFetchCovidStatisticsService
    {
        Task<Stream> FetchTestNumbersFromDate(DateTime date);
        Task<Stream> FetchHospitalNumbersFromDate(DateTime date);
        Task<Stream> FetchVaccineNumbersFromDate(DateTime date);

        Task<Stream> FetchLatestTestNumbers();
        Task<Stream> FetchLatestHospitalNumbers();
        Task<Stream> FetchLatestVaccineNumbers();
    }
}
