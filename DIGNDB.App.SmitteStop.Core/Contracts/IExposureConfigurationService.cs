using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureConfigurationService
    {
        void SetConfiguration(IConfiguration configuration);
        ExposureConfiguration GetConfiguration();
        ExposureConfigurationV1_2 GetConfigurationR1_2();
    }
}
