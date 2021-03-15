using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Mappings
{
    public sealed class TestedMap : CovidStatisticsClassMapBase<TestedCsvContent>
    {
        private const string PositiveColumnName = "n_pos";
        private const string NegativeColumnName = "n_neg";
        public TestedMap()
        {
            Map(m => m.Positive).Name(PositiveColumnName);
            Map(m => m.Negative).Name(NegativeColumnName);
        }
    }
}