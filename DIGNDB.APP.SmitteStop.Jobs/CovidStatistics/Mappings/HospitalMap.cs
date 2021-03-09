using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Mappings
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