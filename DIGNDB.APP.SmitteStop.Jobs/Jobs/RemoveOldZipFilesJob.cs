using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class RemoveOldZipFilesJob : IRemoveOldZipFilesJob
    {
        private readonly IZipFileInfoService _zipFileInfoService;
        private readonly IFileSystem _fileSystem;

        public RemoveOldZipFilesJob(IFileSystem fileSystem, IZipFileInfoService zipFileInfoService)
        {
            _zipFileInfoService = zipFileInfoService;
            _fileSystem = fileSystem;
        }

        public void RemoveOldZipFiles(HangfireConfig hangfireConfig)
        {
            var daysToInvalidateZipFile = hangfireConfig.DaysToInvalidateZipFile;
            var originCountyCode = hangfireConfig.OriginCountryCode;
            foreach (var zipFilesFolder in hangfireConfig.ZipFilesFolders)
            {
                if (string.IsNullOrEmpty(zipFilesFolder))
                {
                    continue;
                }

                RemoveOldZipFilesFromFolder(zipFilesFolder, originCountyCode, daysToInvalidateZipFile);
            }
        }

        private void RemoveOldZipFilesFromFolder(string zipFilesFolder, string originCountyCode, int daysToInvalidateZipFile)
        {
            RemoveOldZipFilesFromSubfolder(_fileSystem.JoinPaths(zipFilesFolder, ZipFileOrigin.All.ToString().ToLower()), daysToInvalidateZipFile);
            RemoveOldZipFilesFromSubfolder(_fileSystem.JoinPaths(zipFilesFolder, originCountyCode.ToLower()), daysToInvalidateZipFile);
        }

        private void RemoveOldZipFilesFromSubfolder(string path, int daysToInvalidateZipFile)
        {
            var deleteFilesOlderThanDate = DateTime.UtcNow.Date.AddDays(-daysToInvalidateZipFile);
            var allFileNames = _fileSystem.GetFileNamesFromDirectory(path);
            var oldZipFileNames = allFileNames.Where(x => _zipFileInfoService.CreateZipFileInfoFromPackageName(new FileInfo(x).Name).PackageDate < deleteFilesOlderThanDate);
            foreach (var filename in oldZipFileNames)
            {
                _fileSystem.DeleteFile(filename);
            }
        }
    }
}
