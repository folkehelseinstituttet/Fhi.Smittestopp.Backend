using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Dto;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface ICovidStatisticsFilePackageBuilder
    {
        FileStreamsPackageDto GetCovidStatisticsFilesPackage();
    }
}