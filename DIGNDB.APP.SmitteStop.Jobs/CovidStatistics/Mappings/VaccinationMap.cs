using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Mappings
{
    public sealed class VaccinationMap : CovidStatisticsClassMapBase<VaccinatedCsvContent>
    {
        private const string FirstDoseColumnName = "cum_pr100_dose_1";
        private const string SecondDoseColumnName = "cum_pr100_dose_2";
        private const string RegionColumnName = "location_code";
        public VaccinationMap()
        {
            Map(m => m.FirstDose).Name(FirstDoseColumnName);
            Map(m => m.SecondDose).Name(SecondDoseColumnName);
            Map(m => m.Region).Name(RegionColumnName);
        }
    }
}