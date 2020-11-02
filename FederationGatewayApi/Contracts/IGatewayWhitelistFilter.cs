using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayWhitelistFilter
    {
        Task FilterKeys([NotNull] IList<TemporaryExposureKey> keys);
    }
}