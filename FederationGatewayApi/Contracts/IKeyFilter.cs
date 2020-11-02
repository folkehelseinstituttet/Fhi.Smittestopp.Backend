using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FederationGatewayApi.Contracts
{
    public interface IKeyFilter
    {
        IList<TemporaryExposureKey> ValidateKeys(IList<TemporaryExposureKey> temporaryExposureKeys, out IList<string> validationErrors);

        IList<TemporaryExposureKey> MapKeys(IList<TemporaryExposureKeyGatewayDto> temporaryExposureKeys);

        Task<IList<TemporaryExposureKey>> RemoveKeyDuplicatesAsync(IList<TemporaryExposureKey> keys);
    }
}
