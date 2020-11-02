using DIGNDB.App.SmitteStop.Core.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string directoryName)
        {
            Directory.CreateDirectory(directoryName);
        }

        public DateTime GetCreationDateUTC(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo.CreationTimeUtc;
        }

        public string[] GetFilenamesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        public string JoinPaths(params string[] paths)
        {
            return Path.Join(paths);
        }

        public void DeleteFile(string filename)
        {
            var fileInfo = new FileInfo(filename);
            fileInfo.Delete();
        }

        public void WriteAllBytes(string filename, byte[] fileContent)
        {
            File.WriteAllBytes(filename, fileContent);
        }

        public bool FileExists(string filename)
        {
            return (File.Exists(filename));
        }

        public byte[] ReadFile(string filename)
        {
            return (File.ReadAllBytes(filename));
        }
    }
}
