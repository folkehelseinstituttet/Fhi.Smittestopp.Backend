using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public class WebServiceWrapper : IWebServiceWrapper
    {

        private readonly HttpClient httpClient;

        public WebServiceWrapper()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await httpClient.GetAsync(url);
        }
    }
}
