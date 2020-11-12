using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Globalization;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class ExposureConfigurationService : IExposureConfigurationService
    {
        private ExposureConfiguration _exposureConfiguration;
        private ExposureConfigurationV1_2 _exposureConfigurationV1_2;

        public ExposureConfigurationService()
        {
            _exposureConfiguration = new ExposureConfiguration();
        }
        public ExposureConfiguration RetrieveExposureConfigurationFromConfig(IConfiguration configuration, string configSectionName)
        {
            ExposureConfiguration exposureConfiguration = new ExposureConfiguration();
            // TODO But why?
            exposureConfiguration.AttenuationScores = configuration.GetSection(configSectionName)
                .GetSection("AttenuationScores").GetChildren().Select(x => int.Parse(x.Value)).ToArray();
            exposureConfiguration.AttenuationWeight = int.Parse(configuration[$"{configSectionName}:AttenuationWeight"]);
            exposureConfiguration.DaysSinceLastExposureScores = configuration.GetSection($"{configSectionName}")
                .GetSection("DaysSinceLastExposureScores").GetChildren().Select(x => int.Parse(x.Value)).ToArray();
            exposureConfiguration.DaysSinceLastExposureWeight =
                int.Parse(configuration[$"{configSectionName}:DaysSinceLastExposureWeight"]);
            exposureConfiguration.MinimumRiskScore = int.Parse(configuration[$"{configSectionName}:MinimumRiskScore"]);
            exposureConfiguration.DurationAtAttenuationThresholds = configuration.GetSection($"{configSectionName}")
                .GetSection("DurationAtAttenuationThresholds").GetChildren().Select(x => int.Parse(x.Value)).ToArray();
            exposureConfiguration.DurationScores = configuration.GetSection($"{configSectionName}")
                .GetSection("DurationScores").GetChildren().Select(x => int.Parse(x.Value)).ToArray();
            exposureConfiguration.DurationWeight = int.Parse(configuration[$"{configSectionName}:DurationWeight"]);
            exposureConfiguration.TransmissionRiskScores = configuration.GetSection($"{configSectionName}")
                .GetSection("TransmissionRiskScores").GetChildren().Select(x => int.Parse(x.Value)).ToArray();
            exposureConfiguration.TransmissionRiskWeight =
                int.Parse(configuration[$"{configSectionName}:TransmissionRiskWeight"]);

            return exposureConfiguration;
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            _exposureConfiguration = RetrieveExposureConfigurationFromConfig(configuration, "ExposureConfig");
            _exposureConfigurationV1_2 = new ExposureConfigurationV1_2()
            {
                Configuration = RetrieveExposureConfigurationFromConfig(configuration, "ExposureConfigV1_2"),
                AttenuationBucketsParams = RetrieveAttentuationBucketsParametersFromConfig(configuration)
            };
        }

        public ExposureConfiguration GetConfiguration()
        {
            return _exposureConfiguration ?? throw new ArgumentException("Exposure Configuration is not initialized");
        }

        public ExposureConfigurationV1_2 GetConfigurationR1_2()
        {
            return _exposureConfigurationV1_2;
        }

        private AttenuationBucketsParams RetrieveAttentuationBucketsParametersFromConfig(IConfiguration configuration)
        {
            AttenuationBucketsParams config = new AttenuationBucketsParams();
            var sectionName = "AttenuationBucketsParams";
            var a = configuration[$"{sectionName}:ExposureTimeThreshold"];
            config.ExposureTimeThreshold = double.Parse(configuration[$"{sectionName}:ExposureTimeThreshold"], CultureInfo.InvariantCulture);
            config.HighAttenuationBucketMultiplier = double.Parse(configuration[$"{sectionName}:HighAttenuationBucketMultiplier"], CultureInfo.InvariantCulture);
            config.LowAttenuationBucketMultiplier = double.Parse(configuration[$"{sectionName}:LowAttenuationBucketMultiplier"], CultureInfo.InvariantCulture);
            config.MiddleAttenuationBucketMultiplier = double.Parse(configuration[$"{sectionName}:MiddleAttenuationBucketMultiplier"], CultureInfo.InvariantCulture);
            return config;
        }
    }
}
