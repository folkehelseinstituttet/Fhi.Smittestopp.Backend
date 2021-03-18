namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class TimeLocationCsvContent : CovidStatisticCsvFileContent
    {
        public const string NorwayRegionName = "norge";
        public string Region { get; set; }
        public int ConfirmedCasesToday { get; set; }
    }
}