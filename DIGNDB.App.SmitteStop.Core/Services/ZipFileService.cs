using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class ZipFileService : IZipFileService
    {
        private IPackageBuilderService _packageBuilder;
        private IZipFileInfoService _zipFileInfoService;
        private readonly IFileSystem _fileSystem;
        private readonly string _rootZipFilesFolder;
        DateTime _currentDateTime;
        private string _currentZipFilesFolder;
        public ZipFileService(IConfiguration configuration, IPackageBuilderService packageBuilder,
            IZipFileInfoService zipFileInfoService, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _rootZipFilesFolder = configuration["ZipFilesFolder"];
            _packageBuilder = packageBuilder;
            _zipFileInfoService = zipFileInfoService;
        }

        public void UpdateZipFiles(DateTime lastCreationDate, DateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
            CreateZipFilesDirectoriesIfNotExists(_rootZipFilesFolder);
            HandleZipFilesForOrigin(ZipFileOrigin.Dk, lastCreationDate);
            HandleZipFilesForOrigin(ZipFileOrigin.All, lastCreationDate);
        }

        private void HandleZipFilesForOrigin(ZipFileOrigin origin, DateTime packageBeginDateTime)
        {
            try
            {
                _currentZipFilesFolder = _fileSystem.JoinPaths(_rootZipFilesFolder, origin.ToString());
                string[] paths = _fileSystem.GetFilenamesFromDirectory(_currentZipFilesFolder);
                int nextBatchNumber = _zipFileInfoService.GetNextBatchNumberForGivenDay(paths, _currentDateTime);
                var zipFilesContents = _packageBuilder.BuildPackageContentV2(packageBeginDateTime, origin);
                CreateZipFiles(origin, zipFilesContents, nextBatchNumber);
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
            return _fileSystem.JoinPaths(_currentZipFilesFolder, $"{_currentDateTime.Date:yyyy-MM-dd}_{nextBatchNumber}_{origin.ToString().ToLower()}.zip");
        }

        private void CreateZipFileSingleBatch(byte[] zipFileContent, string zipFilePath)
        {
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