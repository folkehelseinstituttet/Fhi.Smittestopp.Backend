using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Dto;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface ICovidStatisticsFilePackageBuilder
    {
        FileStreamsPackageDto GetCovidStatisticsFilesPackage();
    }
}