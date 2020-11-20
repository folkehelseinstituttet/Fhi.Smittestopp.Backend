using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;
using System.IO;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class DatabaseKeysToBinaryStreamMapperService : IDatabaseKeysToBinaryStreamMapperService
    {
        private readonly IExposureKeyMapper _exposureKeyMapper;
        private readonly IZipPackageBuilderConfig _appSettingsConfig;

        public DatabaseKeysToBinaryStreamMapperService(IExposureKeyMapper exposureKeyMapper, IZipPackageBuilderConfig appSettingsConfig)
        {
            _exposureKeyMapper = exposureKeyMapper;
            _appSettingsConfig = appSettingsConfig;
        }

        public Stream ExportDiagnosisKeys(IList<TemporaryExposureKey> keys)
        {
            var exportBatch = _exposureKeyMapper.FromEntityToProtoBatch(keys);
            var exportUtil = new ExposureBatchFileUtil(_appSettingsConfig.ZipCertificatePath);
            var task = exportUtil.CreateSignedFileAsync(exportBatch);
            task.Wait();
            return task.Result;
        }
    }
}
