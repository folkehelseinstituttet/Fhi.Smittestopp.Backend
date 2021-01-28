using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayownloadKeyService
    {
        Task DownloadKeysAsync(List<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);

        List<TemporaryExposureKey> FilterKeys(List<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);
    }
}
