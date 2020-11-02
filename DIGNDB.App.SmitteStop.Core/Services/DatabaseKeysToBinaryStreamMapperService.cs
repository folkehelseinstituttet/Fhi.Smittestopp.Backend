using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class DatabaseKeysToBinaryStreamMapperService : IDatabaseKeysToBinaryStreamMapperService
    {
        private readonly IExposureKeyMapper _exposureKeyMapper;
        private readonly IConfiguration _configuration;

        public DatabaseKeysToBinaryStreamMapperService(IExposureKeyMapper exposureKeyMapper, IConfiguration configuration)
        {
            _exposureKeyMapper = exposureKeyMapper;
            _configuration = configuration;
        }

        public Stream ExportDiagnosisKeys(IList<TemporaryExposureKey> keys)
        {
            var exportBatch = _exposureKeyMapper.FromEntityToProtoBatch(keys);
            var exportUtil = new ExposureBatchFileUtil(_configuration["AppSettings:certificateThumbprint"]);
            var task = exportUtil.CreateSignedFileAsync(exportBatch);
            task.Wait();
            return task.Result;
        }
    }
}
