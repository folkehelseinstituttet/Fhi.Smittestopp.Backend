using Hangfire.Storage;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IHealthCheckHangFireService
    {
        public IMonitoringApi GetHangFireMonitoringApi();
    }
}
