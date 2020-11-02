using Newtonsoft.Json;
using System.Collections.Generic;

namespace FederationGatewayApi.Models
{
    public class TemporaryExposureKeyGatewayBatchDto
    {
        [JsonProperty("keys")]
        public IList<TemporaryExposureKeyGatewayDto> Keys { get; set; } = new List<TemporaryExposureKeyGatewayDto>();
    }
}
