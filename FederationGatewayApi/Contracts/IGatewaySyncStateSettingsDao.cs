using DIGNDB.App.SmitteStop.Domain.Settings;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewaySyncStateSettingsDao
    {
        /// <summary>
        /// Returns a single GatewaySyncState object composed from multiple settings. Default values will be returned if no settings were saved before.
        /// </summary>
        /// <returns>GatewaySyncState. Not null.</returns>
        GatewayUploadState GetUploadState();

        GatewayDownloadState GetDownloadState();

        void Save(GatewayUploadState lastSyncState);

        void Save(GatewayDownloadState lastSyncState);
    }
}
