using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IAddTemporaryExposureKeyService
    {
        Task CreateKeysInDatabase(TemporaryExposureKeyBatchDto parameters);

        Task<IList<TemporaryExposureKey>> GetFilteredKeysEntitiesFromDTO(TemporaryExposureKeyBatchDto parameters);
    }
}
