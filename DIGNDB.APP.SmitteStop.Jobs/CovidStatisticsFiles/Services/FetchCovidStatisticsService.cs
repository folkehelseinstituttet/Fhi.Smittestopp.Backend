using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class FetchCovidStatisticsService : IFetchCovidStatisticsService
    {
        private readonly IFileSystem _fileSystem;
        private readonly GetCovidStatisticsJobConfig _config;

        private const string TestedFileName = "data_covid19_lab_by_time_";
        private const string HospitalAdmissionsFileName = "data_covid19_hospital_by_time_";
        private const string VaccinationFileName = "data_covid19_sysvak_by_time_location_";
        private const string TimeLocationFileName = "data_covid19_msis_by_time_location_";
        private const string LocationFileName = "data_covid19_msis_by_location_";
        private const string DemographicsFileName = "data_covid19_demographics_";

        public FetchCovidStatisticsService(IFileSystem fileSystem, GetCovidStatisticsJobConfig config)
        {
            _fileSystem = fileSystem;
            _config = config;
        }

        public Stream FetchTestNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{TestedFileName}{dateFormatted}.csv";
            var stream = GetStreamOrThrow(filePath);

            return stream;
        }

        public Stream FetchHospitalNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{HospitalAdmissionsFileName}{dateFormatted}.csv";
            var stream = GetStreamOrThrow(filePath);

            return stream;
        }

        public Stream FetchVaccineNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{VaccinationFileName}{dateFormatted}.csv";

            return GetStreamOrThrow(filePath);
        }

        public Stream FetchConfirmedCasesTodayNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{TimeLocationFileName}{dateFormatted}.csv";

            return GetStreamOrThrow(filePath);
        }

        public Stream FetchConfirmedCasesTotalNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{LocationFileName}{dateFormatted}.csv";

            return GetStreamOrThrow(filePath);
        }

        public Stream FetchDeathsCasesTotalNumbersFromDate(DateTime date)
        {
            var dateFormatted = $"{date:yyyy-MM-dd}";
            var filePath = $"{_config.CovidStatisticsFolder}/{DemographicsFileName}{dateFormatted}.csv";

            return GetStreamOrThrow(filePath);
        }

        private Stream GetStreamOrThrow(string path)
        {
            var stream = _fileSystem.GetFileStream(path);
            return stream;
        }
    }
}
