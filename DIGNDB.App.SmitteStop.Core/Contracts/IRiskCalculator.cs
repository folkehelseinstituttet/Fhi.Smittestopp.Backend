using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IRiskCalculator
    {
        RiskLevel CalculateRiskLevel(TemporaryExposureKey exposureKey);
    }
}
