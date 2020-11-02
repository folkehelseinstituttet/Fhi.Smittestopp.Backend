using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IDatabaseKeysToBinaryStreamMapperService
    {
        Stream ExportDiagnosisKeys(IList<TemporaryExposureKey> keys);
    }
}
