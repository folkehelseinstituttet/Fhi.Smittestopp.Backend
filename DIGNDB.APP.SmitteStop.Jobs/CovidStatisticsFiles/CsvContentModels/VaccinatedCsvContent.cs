namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class VaccinatedCsvContent : CovidStatisticCsvFileContent
    {
        public const string NorwayRegionName = "norge";
        public double FirstDose { get; set; }
        public double SecondDose { get; set; }
        public string Region { get; set; }
    }
}