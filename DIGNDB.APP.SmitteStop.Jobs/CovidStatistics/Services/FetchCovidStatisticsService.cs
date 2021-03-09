using System;
using System.IO;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public class FetchCovidStatisticsService : IFetchCovidStatisticsService
    {

        private readonly IWebServiceWrapper keyNumbersWebService;

        private const string BASE_URL = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/";
        private const string TESTED_URL = "data_covid19_lab_by_time_";
        private const string HOSPITAL_ADMISSIONS_URL = "data_covid19_hospital_by_time_";
        private const string VACCINATION_URL = "data_covid19_sysvak_by_time_location_";

        public FetchCovidStatisticsService(IWebServiceWrapper webService)
        {
            keyNumbersWebService = webService;
        }

        public async Task<Stream> FetchTestNumbersFromDate(DateTime date)
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{TESTED_URL}{date:yyyy-MM-dd}.csv").Result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> FetchHospitalNumbersFromDate(DateTime date)
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{HOSPITAL_ADMISSIONS_URL}{date:yyyy-MM-dd}.csv").Result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> FetchVaccineNumbersFromDate(DateTime date)
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{VACCINATION_URL}{date:yyyy-MM-dd}.csv").Result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> FetchLatestTestNumbers()
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{TESTED_URL}latest.csv").Result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> FetchLatestHospitalNumbers()
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{HOSPITAL_ADMISSIONS_URL}latest.csv").Result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> FetchLatestVaccineNumbers()
        {
            return await keyNumbersWebService.GetAsync($"{BASE_URL}{VACCINATION_URL}latest.csv").Result.Content.ReadAsStreamAsync();
        }
    }
}
