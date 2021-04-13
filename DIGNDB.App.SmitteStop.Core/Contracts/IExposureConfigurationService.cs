using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureConfigurationService
    {
        ExposureConfiguration GetConfiguration();

        ExposureConfigurationV1_2 GetConfigurationR1_2();

        DailySummaryExposureConfiguration GetDailySummaryConfiguration();
    }
}
