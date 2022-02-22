using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IImportantInfoService
    {
        public ImportantInfoResponse ParseConfig(ImportantInfoRequest request);
        public bool ConfigFileExists();
    }
}
