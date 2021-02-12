using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayWhitelistFilter
    {
        Task FilterKeys([NotNull] IList<TemporaryExposureKey> keys);
    }
}