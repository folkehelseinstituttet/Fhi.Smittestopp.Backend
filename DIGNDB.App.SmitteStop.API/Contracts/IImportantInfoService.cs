using System.IO;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API.Config;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IImportantInfoService
    {
        public ImportantInfoResponse ParseConfig(ImportantInfoRequest request);
        public bool ConfigFileExists();
    }
}
