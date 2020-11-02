using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using System.Collections.Generic;

namespace FederationGatewayApi.Contracts
{
    public interface IEFGSKeyStoreService
    {
        IList<TemporaryExposureKey> FilterAndSaveKeys(IList<TemporaryExposureKeyGatewayDto> keys);
    }
}
