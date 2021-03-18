using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class VaccinationMap : CovidStatisticsClassMapBase<VaccinatedCsvContent>
    {
        private const string FirstDoseColumnName = "n_dose_1";
        private const string SecondDoseColumnName = "n_dose_2";
        private const string FirstDoseTotalColumnName = "cum_n_dose_1";
        private const string SecondDoseTotalColumnName = "cumn_dose_2";
        private const string RegionColumnName = "location_code";
        public VaccinationMap()
        {
            Map(m => m.FirstDose).Name(FirstDoseColumnName);
            Map(m => m.SecondDose).Name(SecondDoseColumnName);
            Map(m => m.FirstDoseTotal).Name(FirstDoseTotalColumnName);
            Map(m => m.SecondDoseTotal).Name(SecondDoseTotalColumnName);
            Map(m => m.Region).Name(RegionColumnName);
        }
    }
}