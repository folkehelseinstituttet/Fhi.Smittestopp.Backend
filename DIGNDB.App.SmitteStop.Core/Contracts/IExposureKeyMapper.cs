using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureKeyMapper
    {
        List<TemporaryExposureKey> FromDtoToEntity(TemporaryExposureKeyBatchDto dto);

        Domain.Proto.TemporaryExposureKey FromEntityToProto(TemporaryExposureKey source);

        Domain.Proto.TemporaryExposureKeyExport FromEntityToProtoBatch(IList<TemporaryExposureKey> dtoKeys);
    }
}
