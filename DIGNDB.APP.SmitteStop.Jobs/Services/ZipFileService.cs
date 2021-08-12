using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using System;
using System.Collections.Generic;
using System.Security;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    public class ZipFileService : IZipFileService
    {
        private readonly string _allKeysFilePostfix = ZipFileOrigin.All.ToString();
        private readonly string _originCountryCode;

        private IPackageBuilderService _packageBuilder;
        private IZipFileInfoService _zipFileInfoService;
        private readonly IFileSystem _fileSystem;
        private readonly List<string> _rootZipFilesFolders;
        private List<string> _filesToBeCommited = new List<string>();
        DateTime _currentDateTime;
        private string _currentZipFilesFolder;
        private const char _temporaryFileMarker = '.';

        public ZipFileService(HangfireConfig hangfireConfig, IPackageBuilderService packageBuilder,
            IZipFileInfoService zipFileInfoService, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _rootZipFilesFolders = hangfireConfig.ZipFilesFolders;
            _originCountryCode = hangfireConfig.OriginCountryCode;
            _packageBuilder = packageBuilder;
            _zipFileInfoService = zipFileInfoService;
        }

        public void UpdateZipFiles(DateTime lastCreationDate, DateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;

            foreach (var folderPath in _rootZipFilesFolders)
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    continue;
                }

                CreateZipFilesDirectoriesIfNotExists(folderPath, originCountryKeysDirectoryName: _originCountryCode, allKeysDirectoryName: _allKeysFilePostfix);
            }

            HandleZipFilesForOrigin(originPostfix: _originCountryCode, packageBeginDateTime: lastCreationDate);
            HandleZipFilesForOrigin(originPostfix: _allKeysFilePostfix, packageBeginDateTime: lastCreationDate);
            CommitFiles();
        }

        private void CommitFiles()
        {
            foreach (var file in _filesToBeCommited)
            {
                CommitFile(file);
            }
        }

        //This method is used to ensure that we ship the files only if we are able to save them all in specified locations.
        private void CommitFile(string filePath)
        {
            var directory = _fileSystem.GetDirectoryNameFromPath(filePath);
            var file = _fileSystem.GetFileNameFromPath(filePath);
            file = file.Substring(file.IndexOf(_temporaryFileMarker) + 1);
            var newFilePath = _fileSystem.JoinPaths(directory, file);
            _fileSystem.Rename(filePath, newFilePath);
        }

        private void HandleZipFilesForOrigin(string originPostfix, DateTime packageBeginDateTime)
        {
            try
            {
                foreach (var _rootZipFilesFolder in _rootZipFilesFolders)
                {
                    _currentZipFilesFolder = _fileSystem.JoinPaths(_rootZipFilesFolder, originPostfix.ToString());
                    //We get rid of any files that has been created but not renamed. This would be the case if any exception occurred during creation of any zip file
                    ClearTemporaryFiles(_currentZipFilesFolder);
                    string[] paths = _fileSystem.GetFileNamesFromDirectory(_currentZipFilesFolder);
                    int nextBatchNumber = _zipFileInfoService.GetNextBatchNumberForGivenDay(paths, _currentDateTime);
                    var zipFilesContents = _packageBuilder.BuildPackageContentV2(packageBeginDateTime, originPostfix);
                    CreateZipFiles(originPostfix, zipFilesContents, nextBatchNumber);
                }
            }
            catch (SecurityException e)
            {
                throw new SecurityException($"Failed to create zip files due to a detection of a security error.", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create zip files for origin: {originPostfix} and date: {packageBeginDateTime}", innerException: e);
            }
        }

        private void ClearTemporaryFiles(string currentZipFilesFolder)
        {
            var temporaryFileList = _fileSystem.GetAllTemporaryFilesFromFolder(currentZipFilesFolder);
            _fileSystem.DeleteFiles(temporaryFileList);
        }

        private void CreateZipFiles(string originPostfix, List<byte[]> zipFilesContents, int nextBatchNumber)
        {
            foreach (var zipFileContent in zipFilesContents)
            {
                var zipFilePath = GetZipFilePath(originPostfix, nextBatchNumber);
                CreateZipFileSingleBatch(zipFileContent, zipFilePath);
                nextBatchNumber++;
            }
        }

        private string GetZipFilePath(string originPostfix, int nextBatchNumber)
        {
            return _fileSystem.JoinPaths(_currentZipFilesFolder, $"{_temporaryFileMarker}{_currentDateTime.Date:yyyy-MM-dd}_{nextBatchNumber}_{originPostfix.ToLower()}.zip");
        }

        private void CreateZipFileSingleBatch(byte[] zipFileContent, string zipFilePath)
        {
            _filesToBeCommited.Add(zipFilePath);
            _fileSystem.WriteAllBytes(zipFilePath, zipFileContent);
        }

        private void CreateZipFilesDirectoriesIfNotExists(string zipFilesFolder, string originCountryKeysDirectoryName, string allKeysDirectoryName)
        {
            try
            {
                _fileSystem.CreateDirectory(zipFilesFolder);
                var dkZipFilesDirectory = _fileSystem.JoinPaths(zipFilesFolder, originCountryKeysDirectoryName.ToLower());
                _fileSystem.CreateDirectory(dkZipFilesDirectory);
                var allZipFilesDirectory = _fileSystem.JoinPaths(zipFilesFolder, allKeysDirectoryName.ToLower());
                _fileSystem.CreateDirectory(allZipFilesDirectory);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create zip files directories with the specified path: {zipFilesFolder}", innerException: e);
            }
        }
    }
}
