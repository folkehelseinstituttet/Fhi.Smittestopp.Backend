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
            package.Files.Add(
                new CsvFileDto()
                {
                    File = _fetchCovidStatisticsService.FetchHospitalNumbersFromDate(_dateTimeResolver.GetDateToday()),
                    Name = CovidStatisticsFileName.Hospital
                });
            package.Files.Add(
                new CsvFileDto()
                {
                    File = _fetchCovidStatisticsService.FetchVaccineNumbersFromDate(_dateTimeResolver.GetDateToday()),
                    Name = CovidStatisticsFileName.Vaccination
                });
            package.Files.Add(
                new CsvFileDto()
                {
                    File = _fetchCovidStatisticsService.FetchTestNumbersFromDate(_dateTimeResolver.GetDateToday()),
                    Name = CovidStatisticsFileName.Test
                });
            return package;
        }
    }
}