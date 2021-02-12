using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class ExposureConfigurationService : IExposureConfigurationService
    {
        private ExposureConfiguration _exposureConfiguration;
        private ExposureConfigurationV1_2 _exposureConfigurationV1_2;

        public ExposureConfigurationService(IConfiguration configuration)
        {
            _exposureConfiguration = RetrieveExposureConfigurationFromConfig(configuration.GetSection("ExposureConfig"));
            ModelValidator.ValidateContract(_exposureConfiguration);

            _exposureConfigurationV1_2 = new ExposureConfigurationV1_2()
            {
                Configuration = RetrieveExposureConfigurationFromConfig(configuration.GetSection("ExposureConfigV1_2")),
                AttenuationBucketsParams = RetrieveAttentuationBucketsParametersFromConfig(configuration.GetSection("AttenuationBucketsParams"))
            };
            ModelValidator.ValidateContract(_exposureConfigurationV1_2);
        }

        public ExposureConfiguration RetrieveExposureConfigurationFromConfig(IConfiguration configuration)
        {
           return  configuration.Get<ExposureConfiguration>();
        }

        public ExposureConfiguration GetConfiguration()
        {
            return _exposureConfiguration;
        }

        public ExposureConfigurationV1_2 GetConfigurationR1_2()
        {
            return _exposureConfigurationV1_2;
        }

        private AttenuationBucketsParams RetrieveAttentuationBucketsParametersFromConfig(IConfiguration configuration)
        {
            return configuration.Get<AttenuationBucketsParams>();
        }
    }
}
