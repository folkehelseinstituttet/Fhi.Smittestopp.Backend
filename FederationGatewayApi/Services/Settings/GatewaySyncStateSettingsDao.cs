using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Settings;
using FederationGatewayApi.Contracts;
using System;
using System.Text.Json;

namespace FederationGatewayApi.Services.Settings
{
    public class GatewaySyncStateSettingsDao : SettingsDao, IGatewaySyncStateSettingsDao
    {
        private static class Keys
        {
            public const string Prefix = "GatewaySyncState_";
            public const string UpdateState = "UpdateState";
            public const string DownloadState = "DownloadState";
        }

        public GatewaySyncStateSettingsDao(ISettingRepository settingsRepository) : base(Keys.Prefix, settingsRepository)
        {
        }

        /// <summary>
        /// Returns the GatewayUploadState object. Default values will be returned if no settings were saved before.
        /// </summary>
        /// <returns>GatewaySyncState. Not null.</returns>
        public GatewayUploadState GetUploadState()
        {
            GatewayUploadState syncStatusDto = GetStateOrNull<GatewayUploadState>(Keys.UpdateState);
            return syncStatusDto ?? new GatewayUploadState();
        }

        public void Save(GatewayUploadState lastSyncState)
        {
            var jsonString = JsonSerializer.Serialize(lastSyncState);
            SetProperty(Keys.UpdateState, jsonString);
        }


        public GatewayDownloadState GetDownloadState()
        {
            GatewayDownloadState syncStatusDto = GetStateOrNull<GatewayDownloadState>(Keys.DownloadState);
            return syncStatusDto ?? new GatewayDownloadState();
        }

        public void Save(GatewayDownloadState lastSyncState)
        {
            var jsonString = JsonSerializer.Serialize(lastSyncState);
            SetProperty(Keys.DownloadState, jsonString);
        }

        private T GetStateOrNull<T>(string key) where T : class, new()
        {
            T syncStatusDto = null;
            var jsonString = FindSettingsProperty(key);
            if (jsonString != null)
            {
                try
                {
                    syncStatusDto = JsonSerializer.Deserialize<T>(jsonString);
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Setting for {GetFullPropertyKey(key)} is corrupted! Cannot load setting value form the DB.", e);
                }
            }
            return syncStatusDto;
        }
    }
}
