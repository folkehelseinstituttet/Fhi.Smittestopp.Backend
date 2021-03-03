using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    public interface IKeyNumbersWebService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
