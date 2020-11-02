using DIGNDB.App.SmitteStop.DAL.Repositories;

namespace FederationGatewayApi.Services.Settings
{
    public abstract class SettingsDao
    {
        private readonly ISettingRepository _settingsRepository;
        protected readonly string _settingsGroupPrefix;

        public SettingsDao(string settingsGroupPrefix, ISettingRepository settingsRepository)
        {
            _settingsGroupPrefix = settingsGroupPrefix;
            _settingsRepository = settingsRepository;
        }

        protected string GetFullPropertyKey(string propertyKey)
        {
            return _settingsGroupPrefix + propertyKey;
        }

        protected string FindSettingsProperty(string propertyKey, string defaultValue = null)
        {
            var fullPropertyKey = GetFullPropertyKey(propertyKey);
            var settingProperty = _settingsRepository.FindSettingByKey(fullPropertyKey);
            if (settingProperty != null)
            {
                // property object exists - check if value column is not null
                return settingProperty.Value ?? defaultValue;
            }
            return defaultValue;
        }

        protected void SetProperty(string propertyKey, string value)
        {
            var fullPropertyKey = GetFullPropertyKey(propertyKey);
            _settingsRepository.SetSetting(fullPropertyKey, value);
        }

    }
}
