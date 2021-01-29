using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IKeyValidator
    {
        bool ValidateKeyGateway(TemporaryExposureKey exposureKey, out string errorMessage);
        bool ValidateKeyAPI(TemporaryExposureKey exposureKey, out string errorMessage);
    }
}
