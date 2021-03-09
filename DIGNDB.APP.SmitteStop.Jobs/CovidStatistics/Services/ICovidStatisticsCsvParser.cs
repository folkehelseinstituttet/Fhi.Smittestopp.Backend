using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Dto;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public interface ICovidStatisticsCsvParser
    {
        CovidStatisticsCsvContent ParsePackage(FileStreamsPackageDto package);
    }
}