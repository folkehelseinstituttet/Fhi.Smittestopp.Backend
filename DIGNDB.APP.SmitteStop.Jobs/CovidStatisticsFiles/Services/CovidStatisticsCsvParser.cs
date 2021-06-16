using CsvHelper;
using CsvHelper.Configuration;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Dto;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Enums;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsCsvParser : ICovidStatisticsCsvParser
    {
        private readonly ILogger<CovidStatisticsCsvParser> _logger;
        private readonly GetCovidStatisticsJobConfig _settings;
        private readonly IDateTimeResolver _dateTimeResolver;

        public CovidStatisticsCsvParser(ILogger<CovidStatisticsCsvParser> logger, GetCovidStatisticsJobConfig settings, IDateTimeResolver dateTimeResolver)
        {
            _dateTimeResolver = dateTimeResolver;
            _settings = settings;
            _logger = logger;
        }

        public CovidStatisticsCsvContent ParsePackage(FileStreamsPackageDto package)
        {
            var parsedPackage = new CovidStatisticsCsvContent();

            var hospitalFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.Hospital);
            var hospitalFileContent = RetrieveDataFromCsvFile(hospitalFile, new HospitalMap());
            parsedPackage.AddFileContent(hospitalFileContent);

            var testedFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.Test);
            var testedFileContent = RetrieveDataFromCsvFile(testedFile, new TestedMap());
            parsedPackage.AddFileContent(testedFileContent);

            var vaccinationFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.Vaccination);
            var vaccinationFileContent = RetrieveDataFromCsvFile(vaccinationFile, new VaccinationMap());
            parsedPackage.AddFileContent(vaccinationFileContent);

            var confirmedTodayFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.ConfirmedToday);
            var confirmedTodayFileContent = RetrieveDataFromCsvFile(confirmedTodayFile, new ConfirmedCasesTodayMap());
            parsedPackage.AddFileContent(confirmedTodayFileContent);

            var confirmedTotalFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.ConfirmedTotal);
            var confirmedTotalFileContent = RetrieveDataFromCsvFile(confirmedTotalFile, new ConfirmedCasesTotalMap());
            parsedPackage.AddFileContent(confirmedTotalFileContent);

            var deathTotalFile = GetFileOrThrow(package.Files, CovidStatisticsFileName.DeathsTotal);
            var deathTotalFileContent = RetrieveDataFromCsvFile(deathTotalFile, new DeathsCasesTotalMap());
            parsedPackage.AddFileContent(deathTotalFileContent);

            return parsedPackage;
        }

        private IEnumerable<T> RetrieveDataFromCsvFile<T>(Stream csvStream, ClassMap<T> classMap)
        {
            using var streamReader = GetStreamReaderForStream(csvStream);
            using var csvContentReader = ConfigureCsvReader(classMap, streamReader);
            try
            {
                var records = csvContentReader.GetRecords<T>().ToList();
                return records;
            }
            catch (Exception e)
            {
                _logger.LogError(CovidStatisticsParsingException.ExceptionMessage);
                throw new CovidStatisticsParsingException(e);
            }
        }

        private StreamReader GetStreamReaderForStream(Stream csvStream)
        {
            try
            {
                return new StreamReader(csvStream);
            }
            catch (Exception e)
            {
                throw new CovidStatisticsStreamProcessingException(e);
            }
        }

        private CsvReader ConfigureCsvReader<T>(ClassMap<T> classMap, StreamReader streamReader)
        {
            var csvContentReader = new CsvReader(streamReader, new CultureInfo("no-NO"));
            csvContentReader.Configuration.RegisterClassMap(classMap);
            csvContentReader.Configuration.CultureInfo = CultureInfo.InvariantCulture;
            csvContentReader.Configuration.Delimiter = ",";
            return csvContentReader;
        }

        private static Stream GetFileOrThrow(List<CsvFileDto> packageFiles, CovidStatisticsFileName fileName)
        {
            var retVal = packageFiles.SingleOrDefault(x => x.Name == fileName)?.File ??
                         throw new CovidStatisticsParsingFileNotFoundException(fileName);
            return retVal;
        }
    }
}