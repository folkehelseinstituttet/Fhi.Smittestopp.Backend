namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels
{
    public class HospitalCsvContent : CovidStatisticCsvFileContent
    {
        public int HospitalAdmitted { get; set; }
        public int IcuPatients { get; set; }
    }
}