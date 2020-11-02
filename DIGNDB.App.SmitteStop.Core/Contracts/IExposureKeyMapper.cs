using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureKeyMapper
    {
         List<TemporaryExposureKey> FromDtoToEntity(TemporaryExposureKeyBatchDto dto);

         IList<TemporaryExposureKey> FilterDuplicateKeys(IList<TemporaryExposureKey> incomingKeys, IList<TemporaryExposureKey> exsitingKeys);

        Domain.Proto.TemporaryExposureKey FromEntityToProto(TemporaryExposureKey source);

        Domain.Proto.TemporaryExposureKeyExport FromEntityToProtoBatch(IList<TemporaryExposureKey> dtoKeys);
    }
}
