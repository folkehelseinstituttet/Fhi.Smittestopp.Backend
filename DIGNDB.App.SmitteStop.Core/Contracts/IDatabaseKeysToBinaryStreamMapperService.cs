using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.IO;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IDatabaseKeysToBinaryStreamMapperService
    {
        Stream ExportDiagnosisKeys(IList<TemporaryExposureKey> keys);
    }
}
