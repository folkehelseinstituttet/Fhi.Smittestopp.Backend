using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IAddTemporaryExposureKeyService
    {
        Task CreateKeysInDatabase(TemporaryExposureKeyBatchDto parameters, KeySource apiVersion);

        Task<IList<TemporaryExposureKey>> GetFilteredKeysEntitiesFromDTO(TemporaryExposureKeyBatchDto parameters);
    }
}
