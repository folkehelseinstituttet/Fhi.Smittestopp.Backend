using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public class FetchCovidStatisticsService : IFetchCovidStatisticsService
    {

        private readonly IFileSystem _fileSystem;
        private readonly GetCovidStatisticsJobConfig _config;

        private const string TestedFileName = "data_covid19_lab_by_time_";
        private const string HospitalAdmissionsFileName = "data_covid19_hospital_by_time_";
        private const string VaccinationFileName = "data_covid19_sysvak_by_time_location_";

        public FetchCovidStatisticsService(IFileSystem fileSystem, GetCovidStatisticsJobConfig config)
        {
            _fileSystem = fileSystem;
            _config = config;
        }

        public Stream FetchTestNumbersFromDate(DateTime date)
        {
            return GetStreamOrThrow($"{_config.CovidStatisticsFolder}/{TestedFileName}{date:yyyy-MM-dd}.csv");
        }

        public Stream FetchHospitalNumbersFromDate(DateTime date)
        {
            return GetStreamOrThrow(
                $"{_config.CovidStatisticsFolder}/{HospitalAdmissionsFileName}{date:yyyy-MM-dd}.csv");
        }

        public Stream FetchVaccineNumbersFromDate(DateTime date)
        {
            return GetStreamOrThrow($"{_config.CovidStatisticsFolder}/{VaccinationFileName}{date:yyyy-MM-dd}.csv");
        }

        private Stream GetStreamOrThrow(string path)
        {
            return _fileSystem.GetFileStream(path);
        }
    }
}
