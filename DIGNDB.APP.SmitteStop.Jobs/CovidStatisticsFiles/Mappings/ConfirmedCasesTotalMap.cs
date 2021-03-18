using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class ConfirmedCasesTotalMap : CovidStatisticsClassMapBase<LocationCsvContent>
    {
        private const string ConfirmedCasesTotalColumnName = "n";
        private const string RegionColumnName = "location_code";

        public ConfirmedCasesTotalMap()
        {
            Map(m => m.ConfirmedCasesTotal).Name(ConfirmedCasesTotalColumnName);
            Map(m => m.Region).Name(RegionColumnName);
        }
    }
}