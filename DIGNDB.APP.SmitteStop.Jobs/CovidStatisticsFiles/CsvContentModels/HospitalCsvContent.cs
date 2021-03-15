namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class HospitalCsvContent : CovidStatisticCsvFileContent
    {
        public int HospitalAdmitted { get; set; }
        public int IcuPatients { get; set; }
    }
}