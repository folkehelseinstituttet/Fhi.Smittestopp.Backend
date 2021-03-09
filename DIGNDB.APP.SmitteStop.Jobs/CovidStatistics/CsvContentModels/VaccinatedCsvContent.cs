namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels
{
    public class VaccinatedCsvContent : CovidStatisticCsvFileContent
    {
        public double FirstDose { get; set; }
        public double SecondDose { get; set; }
        public string Region { get; set; }
    }
}