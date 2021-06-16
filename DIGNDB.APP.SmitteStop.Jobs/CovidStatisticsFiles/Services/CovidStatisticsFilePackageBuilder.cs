using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Dto;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Enums;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsFilePackageBuilder : ICovidStatisticsFilePackageBuilder
    {
        private readonly IFetchCovidStatisticsService _fetchCovidStatisticsService;
        private readonly IDateTimeResolver _dateTimeResolver;

        public CovidStatisticsFilePackageBuilder(IDateTimeResolver dateTimeResolver, IFetchCovidStatisticsService fetchCovidStatisticsService)
        {
            _dateTimeResolver = dateTimeResolver;
            _fetchCovidStatisticsService = fetchCovidStatisticsService;
        }

        public FileStreamsPackageDto GetCovidStatisticsFilesPackage()
        {
            var package = new FileStreamsPackageDto();

            var today = _dateTimeResolver.GetDateToday();

            var hospitalFile = _fetchCovidStatisticsService.FetchHospitalNumbersFromDate(today);
            var hospitalNumbers = new CsvFileDto {File = hospitalFile, Name = CovidStatisticsFileName.Hospital};
            package.Files.Add(hospitalNumbers);

            var vaccineFile = _fetchCovidStatisticsService.FetchVaccineNumbersFromDate(today);
            var vaccineNumbers = new CsvFileDto {File = vaccineFile, Name = CovidStatisticsFileName.Vaccination};
            package.Files.Add(vaccineNumbers);

            var testFile = _fetchCovidStatisticsService.FetchTestNumbersFromDate(today);
            var testNumbers = new CsvFileDto {File = testFile, Name = CovidStatisticsFileName.Test};
            package.Files.Add(testNumbers);

            var timeLocationFile = _fetchCovidStatisticsService.FetchConfirmedCasesTodayNumbersFromDate(today);
            var timeLocationNumbers = new CsvFileDto
                {File = timeLocationFile, Name = CovidStatisticsFileName.ConfirmedToday};
            package.Files.Add(timeLocationNumbers);

            var locationFile = _fetchCovidStatisticsService.FetchConfirmedCasesTotalNumbersFromDate(today);
            var locationNumbers = new CsvFileDto {File = locationFile, Name = CovidStatisticsFileName.ConfirmedTotal};
            package.Files.Add(locationNumbers);

            var demographicsFile = _fetchCovidStatisticsService.FetchDeathsCasesTotalNumbersFromDate(today);
            var demographicsNumbers = new CsvFileDto { File = demographicsFile, Name = CovidStatisticsFileName.DeathsTotal };
            package.Files.Add(demographicsNumbers);

            return package;
        }
    }
}