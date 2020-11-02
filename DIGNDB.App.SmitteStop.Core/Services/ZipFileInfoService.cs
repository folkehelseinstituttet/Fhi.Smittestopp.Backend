using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class ZipFileInfoService : IZipFileInfoService
    {
        private readonly IFileSystem _fileSystem;

        public ZipFileInfoService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public int GetNextBatchNumberForGivenDay(string[] paths, DateTime currentDateTime)
        {
            var packageNames = GetFilenamesFromPaths(paths);
            var allZipFileInfos = GetZipFileInfosFromPackageNames(packageNames);
            var zipFileInfosForDay = allZipFileInfos.Where(x => x.PackageDate.Date == currentDateTime.Date);
            return zipFileInfosForDay.Count() != 0 ? zipFileInfosForDay.OrderByDescending(x => x.BatchNumber).First().BatchNumber + 1 : 1;
        }

        public ZipFileInfo CreateZipFileInfoFromPackageName(string zipFilename)
        {
            var zipFileInfo = new ZipFileInfo();
            var tokens = zipFilename.Split("_");
            if (tokens.Length == 3)
            {
                var dateString = tokens[0];
                DateTime packageDate;
                int batchNumber = 0;
                bool successfullyParsed = DateTime.TryParse(dateString, out packageDate) && int.TryParse(tokens[1], out batchNumber);
                var originTokens = tokens[2].Split(".");
                if (successfullyParsed && originTokens.Count() == 2)
                {
                    zipFileInfo.Origin = tokens[2].Split(".")[0];
                    zipFileInfo.BatchNumber = batchNumber;
                    zipFileInfo.PackageDate = packageDate;
                    return zipFileInfo;
                }
            }
            throw new FormatException("Unexpected filename format");
        }

        public bool CheckIfPackageExists(ZipFileInfo packageInfo, string zipFilesFolder)
        {
            var filename = _fileSystem.JoinPaths(zipFilesFolder, packageInfo.Origin.ToLower(), packageInfo.FileName);
            return (_fileSystem.FileExists(filename));
        }

        public byte[] ReadPackage(ZipFileInfo packageInfo, string zipFilesFolder)
        {
            var filename = _fileSystem.JoinPaths(zipFilesFolder, packageInfo.Origin.ToLower(), packageInfo.FileName);
            return _fileSystem.ReadFile(filename);
        }

        public List<string> GetFilenamesFromPaths(string[] paths)
        {
            return paths.Select(path => new FileInfo(path)).Select(fileInfo => fileInfo.Name).ToList();
        }

        private List<ZipFileInfo> GetZipFileInfosFromPackageNames(List<string> packageNames)
        {
            return packageNames.Select(CreateZipFileInfoFromPackageName).ToList();
        }
    }
}
