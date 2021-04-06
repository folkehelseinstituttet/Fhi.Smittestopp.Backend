using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Dto.DailySummaryConfiguration;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureConfigurationService
    {
        ExposureConfiguration GetConfiguration();

        ExposureConfigurationV1_2 GetConfigurationR1_2();

        DailySummaryConfiguration GetDailySummaryConfiguration();
    }
}
