using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class DeathsCsvContent : CovidStatisticCsvFileContent
    {
        public static DateTime DeathsDateTime = new DateTime(1900, 01, 01);

        public const string NorwayRegionName = "norge";
        public string Region { get; set; }
        public string Age { get; set; }
        public int DeathsCasesTotal { get; set; }
    }
}