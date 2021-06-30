using Hangfire;
using Hangfire.Storage;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IHealthCheckHangFireService
    {
        //public JobStorage GetJobStorage();
        public IMonitoringApi GetHangFireMonitoringApi();
    }
}
