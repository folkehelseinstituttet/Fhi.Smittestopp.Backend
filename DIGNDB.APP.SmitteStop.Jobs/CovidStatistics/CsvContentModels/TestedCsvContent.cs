namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels
{
    public class TestedCsvContent : CovidStatisticCsvFileContent
    {
        public int Negative { get; set; }
        public int Positive { get; set; }
    }
}