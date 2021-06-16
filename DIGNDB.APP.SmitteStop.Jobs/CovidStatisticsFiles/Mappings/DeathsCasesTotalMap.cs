using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class DeathsCasesTotalMap : CovidStatisticsClassMapBase<DeathsCsvContent>
    {
        private const string DeathsCasesTotalColumnName = "n";
        private const string RegionColumnName = "location_code";
        private const string AgeColumnName = "age";

        public DeathsCasesTotalMap()
        {
            Map(m => m.DeathsCasesTotal).Name(DeathsCasesTotalColumnName);
            Map(m => m.Region).Name(RegionColumnName);
            Map(m => m.Age).Name(AgeColumnName);
        }
    }
}