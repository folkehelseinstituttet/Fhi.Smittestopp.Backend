using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class RemoveOldZipFilesJob : IRemoveOldZipFilesJob
    {
        private int _daysToInvalidateZipFile;
        private readonly IZipFileInfoService _zipFileInfoService;
        private readonly IFileSystem _fileSystem;

        public RemoveOldZipFilesJob(IFileSystem fileSystem, IZipFileInfoService zipFileInfoService)
        {
            _zipFileInfoService = zipFileInfoService;
            _fileSystem = fileSystem;
        }

        public void RemoveOldZipFiles(HangfireConfig hangfireConfig)
        {
            _daysToInvalidateZipFile = hangfireConfig.DaysToInvalidateZipFile;
            foreach (var zipFilesFolder in hangfireConfig.ZipFilesFolders)
            {
                RemoveOldZipFilesFromFolder(zipFilesFolder);
            }
        }

        private void RemoveOldZipFilesFromFolder(string zipFilesFolder)
        {
            RemoveOldZipFilesFromSubfolder(_fileSystem.JoinPaths(zipFilesFolder,ZipFileOrigin.All.ToString().ToLower()));
            RemoveOldZipFilesFromSubfolder(_fileSystem.JoinPaths(zipFilesFolder, ZipFileOrigin.Dk.ToString().ToLower()));
        }

        private void RemoveOldZipFilesFromSubfolder(string path)
        {
            var deleteFilesOlderThanDate = DateTime.UtcNow.Date.AddDays(-_daysToInvalidateZipFile);
            var allFilenames = _fileSystem.GetFilenamesFromDirectory(path);
            var oldZipFilenames = allFilenames.Where(x => _zipFileInfoService.CreateZipFileInfoFromPackageName(new FileInfo(x).Name).PackageDate < deleteFilesOlderThanDate);
            foreach (var filename in oldZipFilenames)
            {
                _fileSystem.DeleteFile(filename);
            }
        }
    }
}
