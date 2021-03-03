using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    public class KeyNumbersWebService : IKeyNumbersWebService
    {

        private readonly HttpClient httpClient;

        public KeyNumbersWebService()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await httpClient.GetAsync(url);
        }
    }
}
