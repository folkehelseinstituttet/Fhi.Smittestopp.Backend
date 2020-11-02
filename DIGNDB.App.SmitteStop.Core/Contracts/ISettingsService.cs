using DIGNDB.App.SmitteStop.Domain.Settings;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface ISettingsService
    {
        GatewayUploadState GetGatewayUploadState();

        GatewayDownloadState GetGatewayDownloadState();

        void SaveGatewaySyncState(GatewayUploadState lastSyncState);

        void SaveGatewaySyncState(GatewayDownloadState lastSyncState);
    }
}
