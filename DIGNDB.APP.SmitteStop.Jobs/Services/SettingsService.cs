using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Settings;
using FederationGatewayApi.Contracts;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    /// <summary>
    /// Service for getting and saving settings objects
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private IGatewaySyncStateSettingsDao _syncStateDao;

        public SettingsService(IGatewaySyncStateSettingsDao syncStateDao)
        {
            _syncStateDao = syncStateDao;
        }

        public GatewayDownloadState GetGatewayDownloadState()
        {
            var dto = _syncStateDao.GetDownloadState();
            ModelValidator.ValidateContract(dto);
            return dto;
        }

        public GatewayUploadState GetGatewayUploadState()
        {
            var dto = _syncStateDao.GetUploadState();
            ModelValidator.ValidateContract(dto);
            return dto;
        }

        public void SaveGatewaySyncState(GatewayUploadState lastSyncState)
        {
            ModelValidator.ValidateContract(lastSyncState);
            _syncStateDao.Save(lastSyncState);
        }

        public void SaveGatewaySyncState(GatewayDownloadState lastSyncState)
        {
            ModelValidator.ValidateContract(lastSyncState);
            _syncStateDao.Save(lastSyncState);
        }
    }
}
