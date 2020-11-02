using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IKeysListToMemoryStreamConverter
    {
        byte[] ConvertKeysToMemoryStream(IList<TemporaryExposureKey> keys);
    }
}