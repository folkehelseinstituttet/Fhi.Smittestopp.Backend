using FederationGatewayApi.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayWebContextReader
    {
        string ReadHttpContextStream(HttpResponseMessage webContext);
        IList<TemporaryExposureKeyGatewayDto> GetItemsFromRequest(string responseBody);

    }
}
