using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class ConfirmedCasesTodayMap : CovidStatisticsClassMapBase<TimeLocationCsvContent>
    {
        private const string ConfirmedCasesTodayColumnName = "n";
        private const string RegionColumnName = "location_code";

        public ConfirmedCasesTodayMap()
        {
            Map(m => m.ConfirmedCasesToday).Name(ConfirmedCasesTodayColumnName);
            Map(m => m.Region).Name(RegionColumnName);
        }
    }
}