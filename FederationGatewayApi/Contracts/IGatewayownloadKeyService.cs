using DIGNDB.App.SmitteStop.Core.Models;
using FederationGatewayApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayownloadKeyService
    {
        Task DownloadKeysAsync(List<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);

        List<TemporaryExposureKey> FilterKeys(List<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);
    }
}
