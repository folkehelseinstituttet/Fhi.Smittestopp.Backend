using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IGatewayHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent requestBody);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }
}