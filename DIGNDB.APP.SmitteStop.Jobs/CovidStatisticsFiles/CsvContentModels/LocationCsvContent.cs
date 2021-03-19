using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class LocationCsvContent : CovidStatisticCsvFileContent
    {
        public static DateTime LocationDateTime = new DateTime(1900, 01, 01);

        public const string NorwayRegionName = "norge";
        public string Region { get; set; }
        public int ConfirmedCasesTotal { get; set; }
    }
}