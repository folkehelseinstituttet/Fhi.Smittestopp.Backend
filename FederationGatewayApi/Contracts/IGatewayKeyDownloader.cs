using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayKeyDownloader
    {
         Task<List<TemporaryExposureKey>> DownloadKeysAsync();

    }
}
