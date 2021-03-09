using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Mappings
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