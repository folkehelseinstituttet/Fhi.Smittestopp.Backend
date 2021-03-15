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
            CovidStatisticsCsvContent parsedPackage = new CovidStatisticsCsvContent();
            parsedPackage.AddFile(RetrieveDataFromCsvFile(GetFileOrThrow(package.Files, CovidStatisticsFileName.Hospital), new HospitalMap()));
            parsedPackage.AddFile(RetrieveDataFromCsvFile(GetFileOrThrow(package.Files, CovidStatisticsFileName.Test), new TestedMap()));
            parsedPackage.AddFile(RetrieveDataFromCsvFile(GetFileOrThrow(package.Files, CovidStatisticsFileName.Vaccination), new VaccinationMap()));
            return parsedPackage;
        }
        private IEnumerable<T> RetrieveDataFromCsvFile<T>(Stream csvStream, ClassMap<T> classMap)
        {
            using var streamReader = GetStreamReaderForStream(csvStream);
            CheckIfDataIsMissing(streamReader, csvStream);
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

        private void CheckIfDataIsMissing(StreamReader streamReader, Stream stream)
        {
            var content = streamReader.ReadToEnd();
            if (content == "404: Not Found")
            {
                HandleDataMissing();
            }
            stream.Position = 0;
        }

        private void HandleDataMissing()
        {
            if (_dateTimeResolver.GetDateTimeNow().Hour < _settings.MakeAlertIfDataIsMissingAfterHour)
            {
                _logger.LogInformation(CovidStatisticsFilleMissingOnServerException.DataMissingInfoMessage);
            }
            else
            {
                _logger.LogError(CovidStatisticsFilleMissingOnServerException.DataMissingErrorMessage);
                throw new CovidStatisticsFilleMissingOnServerException();
            }
        }

        private static Stream GetFileOrThrow(List<CsvFileDto> packageFiles, CovidStatisticsFileName fileName)
        {
            return packageFiles.SingleOrDefault(x => x.Name == fileName)?.File ?? throw new CovidStatisticsParsingFileNotFoundException(fileName);
        }
    }
}