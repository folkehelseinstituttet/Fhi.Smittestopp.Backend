namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class TestedCsvContent : CovidStatisticCsvFileContent
    {
        public int Negative { get; set; }
        public int Positive { get; set; }
    }
}