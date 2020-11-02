using FederationGatewayApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayWebContextReader
    {
        string ReadHttpContextStream(HttpResponseMessage webContext);
        IList<TemporaryExposureKeyGatewayDto> GetItemsFromRequest(string responseBody);

    }
}
