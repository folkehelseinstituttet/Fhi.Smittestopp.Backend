using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    public class ZipFileService : IZipFileService
    {
        private IPackageBuilderService _packageBuilder;
        private IZipFileInfoService _zipFileInfoService;
        private readonly IFileSystem _fileSystem;
        private readonly List<string> _rootZipFilesFolders;
        private List<string> _filesToBeCommited = new List<string>();
        DateTime _currentDateTime;
        private string _currentZipFilesFolder;
        private const char _temporaryFileMarker = '.';
        public ZipFileService(IOptions<HangfireConfig> configuration, IPackageBuilderService packageBuilder,
            IZipFileInfoService zipFileInfoService, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _rootZipFilesFolders = configuration.Value.ZipFilesFolders;
            _packageBuilder = packageBuilder;
            _zipFileInfoService = zipFileInfoService;
        }

        public void UpdateZipFiles(DateTime lastCreationDate, DateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
            foreach (var folderPath in _rootZipFilesFolders)
            {
                CreateZipFilesDirectoriesIfNotExists(folderPath);
            }
            HandleZipFilesForOrigin(ZipFileOrigin.Dk, lastCreationDate);
            HandleZipFilesForOrigin(ZipFileOrigin.All, lastCreationDate);
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

        private void HandleZipFilesForOrigin(ZipFileOrigin origin, DateTime packageBeginDateTime)
        {
            try
            {
                foreach (var _rootZipFilesFolder in _rootZipFilesFolders)
                {
                    _currentZipFilesFolder = _fileSystem.JoinPaths(_rootZipFilesFolder, origin.ToString());
                    //We get rid of any files that has been created but not renamed. This would be the case if any exception occured during creation of any zip file
                    ClearTemporaryFiles(_currentZipFilesFolder);
                    string[] paths = _fileSystem.GetFilenamesFromDirectory(_currentZipFilesFolder);
                    int nextBatchNumber = _zipFileInfoService.GetNextBatchNumberForGivenDay(paths, _currentDateTime);
                    var zipFilesContents = _packageBuilder.BuildPackageContentV2(packageBeginDateTime, origin);
                    CreateZipFiles(origin, zipFilesContents, nextBatchNumber);
                }
            }
            catch (SecurityException e)
            {
                throw new SecurityException($"Failed to create zip files due to a detection of a security error.", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create zip files for origin: {origin} and date: {packageBeginDateTime}", innerException: e);
            }
        }

        private void ClearTemporaryFiles(string currentZipFilesFolder)
        {
            var temporaryFileList = _fileSystem.GetAllTemporaryFilesFromFolder(currentZipFilesFolder);
            _fileSystem.DeleteFiles(temporaryFileList);
        }

        private void CreateZipFiles(ZipFileOrigin origin, List<byte[]> zipFilesContents, int nextBatchNumber)
        {
            foreach (var zipFileContent in zipFilesContents)
            {
                var zipFilePath = GetZipFilePath(origin, nextBatchNumber);
                CreateZipFileSingleBatch(zipFileContent, zipFilePath);
                nextBatchNumber++;
            }
        }

        private string GetZipFilePath(ZipFileOrigin origin, int nextBatchNumber)
        {
            return _fileSystem.JoinPaths(_currentZipFilesFolder, $".{_currentDateTime.Date:yyyy-MM-dd}_{nextBatchNumber}_{origin.ToString().ToLower()}.zip");
        }

        private void CreateZipFileSingleBatch(byte[] zipFileContent, string zipFilePath)
        {
            _filesToBeCommited.Add(zipFilePath);
            _fileSystem.WriteAllBytes(zipFilePath, zipFileContent);
        }

        private void CreateZipFilesDirectoriesIfNotExists(string zipFilesFolder)
        {
            try
            {
                _fileSystem.CreateDirectory(zipFilesFolder);
                var dkZipFilesDirectory = _fileSystem.JoinPaths(zipFilesFolder, ZipFileOrigin.Dk.ToString().ToLower());
                _fileSystem.CreateDirectory(dkZipFilesDirectory);
                var allZipFilesDirectory = _fileSystem.JoinPaths(zipFilesFolder, ZipFileOrigin.All.ToString().ToLower());
                _fileSystem.CreateDirectory(allZipFilesDirectory);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create zip files directories with the specified path: {zipFilesFolder}", innerException: e);
            }
        }
    }
}