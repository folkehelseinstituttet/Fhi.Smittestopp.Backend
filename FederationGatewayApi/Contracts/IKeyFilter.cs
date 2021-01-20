using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using System.Collections.Generic;

namespace FederationGatewayApi.Contracts
{
    public interface IKeyFilter
    {
        IList<TemporaryExposureKey> ValidateKeys(IList<TemporaryExposureKey> temporaryExposureKeys, out IList<string> validationErrors);

        IList<TemporaryExposureKey> MapKeys(IList<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);
    }
}
