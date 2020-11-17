using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class KeysListToMemoryStreamConverter : IKeysListToMemoryStreamConverter
    {
        IDatabaseKeysToBinaryStreamMapperService _databaseKeysToBinaryStreamMapperService;

        public KeysListToMemoryStreamConverter(IDatabaseKeysToBinaryStreamMapperService databaseKeysToBinaryStreamMapperService)
        {
            _databaseKeysToBinaryStreamMapperService = databaseKeysToBinaryStreamMapperService;
        }

        public byte[] ConvertKeysToMemoryStream(IList<TemporaryExposureKey> keys)
        {
            using (Stream stream = _databaseKeysToBinaryStreamMapperService.ExportDiagnosisKeys(keys.ToList()))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(memoryStream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return (memoryStream.ToArray());
                }
            }
        }
    }
}
