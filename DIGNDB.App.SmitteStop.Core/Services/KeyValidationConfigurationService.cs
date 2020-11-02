using System;
using System.Linq;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class KeyValidationConfigurationService : IKeyValidationConfigurationService
    {
        private readonly KeyValidationConfiguration _keyValidationConfiguration;

        public KeyValidationConfigurationService()
        {
            _keyValidationConfiguration = new KeyValidationConfiguration();
        }
        public void SetConfiguration(IConfiguration configuration)
        {
            _keyValidationConfiguration.OutdatedKeysDayOffset  = int.Parse(configuration["KeyValidationRules:OutdatedKeysDayOffset"]);
            _keyValidationConfiguration.Regions = configuration.GetSection("KeyValidationRules")
                .GetSection("Regions")
                .GetChildren()
                .Select(x => x.Value)
                .ToList();
            _keyValidationConfiguration.PackageNames = GetPackageNames(configuration);
        }

        private PackageNameConfig GetPackageNames(IConfiguration configuration)
        {
            var result = new PackageNameConfig();
            result.Google = configuration
                .GetSection("KeyValidationRules")
                .GetSection("PackageNames")
                .GetSection("android")
                .Value;
            result.Apple = configuration
                .GetSection("KeyValidationRules")
                .GetSection("PackageNames")
                .GetSection("ios")
                .Value;
            return result;
        }

        public KeyValidationConfiguration GetConfiguration()
        {
            return _keyValidationConfiguration ?? throw new ArgumentException("Exposure Configuration is not initialized");
        }
    }
}
