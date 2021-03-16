using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class HospitalMap : CovidStatisticsClassMapBase<HospitalCsvContent>
    {
        private const string HospitalAdmittedColumnName = "n_hospital_main_cause";
        private const string IncubatorsColumnName = "n_icu";
        public HospitalMap()
        {
            Map(m => m.HospitalAdmitted).Name(HospitalAdmittedColumnName);
            Map(m => m.IcuPatients).Name(IncubatorsColumnName);
        }
    }
}