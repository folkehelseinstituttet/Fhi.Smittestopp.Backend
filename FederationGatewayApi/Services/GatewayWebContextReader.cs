using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace FederationGatewayApi.Services
{
    public class GatewayWebContextReader : IGatewayWebContextReader
    {
        public string ReadHttpContextStream(HttpResponseMessage webContext)
        {
            using (var reader = new StreamReader(webContext.Content.ReadAsStreamAsync().Result))
            {
                return reader.ReadToEndAsync().Result;
            }
        }

        public IList<TemporaryExposureKeyGatewayDto> GetItemsFromRequest(string responseBody)
        {
            var batchDto = JsonConvert.DeserializeObject<TemporaryExposureKeyGatewayBatchDto>(responseBody);
            return batchDto.Keys ?? new List<TemporaryExposureKeyGatewayDto>();
        }

    }
}
