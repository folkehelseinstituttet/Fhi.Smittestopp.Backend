namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IStatisticsFileService
    {
        public int DeleteOldFiles(string path, int days, string datePattern, string dateParsingFormat);
    }
}
