using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public interface IWebServiceWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
