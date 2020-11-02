using System.IO;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.API
{
    public class AppSettingsConfig : IAppSettingsConfig
    {
        private readonly IConfiguration _configuration;
        public const string AppSettingsSectionName = "AppSettings";

        public AppSettingsConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration => _configuration.GetSection(AppSettingsSectionName);
    }
}
