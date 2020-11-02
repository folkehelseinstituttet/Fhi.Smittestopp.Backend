using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using DIGNDB.App.SmitteStop.Domain;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class ExportKeyConfigurationService : IExportKeyConfigurationService
    {
        private readonly ExportKeyConfiguration _exportKeyConfiguration;

        public ExportKeyConfigurationService()
        {
            _exportKeyConfiguration = new ExportKeyConfiguration();
        }
        public ExportKeyConfiguration GetConfiguration()
        {
            return _exportKeyConfiguration ?? throw new ArgumentException("Export Key Configuration is not initialized");
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            _exportKeyConfiguration.PreviousDayFileCaching = TimeSpan.Parse(configuration["AppSettings:PreviousDayFileCaching"]);
            _exportKeyConfiguration.CurrentDayFileCaching = TimeSpan.Parse(configuration["AppSettings:CurrentDayFileCaching"]);
            _exportKeyConfiguration.MaxKeysPerFile = int.Parse(configuration["AppSettings:MaxKeysPerFile"]);
        }
    }
}
