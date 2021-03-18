namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class VaccinatedCsvContent : CovidStatisticCsvFileContent
    {
        public const string NorwayRegionName = "norge";
        public int FirstDose { get; set; }
        public int SecondDose { get; set; }
        public int FirstDoseTotal { get; set; }
        public int SecondDoseTotal { get; set; }
        public string Region { get; set; }
    }
}