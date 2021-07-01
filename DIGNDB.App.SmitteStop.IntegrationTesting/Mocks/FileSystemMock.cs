using System;
using System.IO;
using DIGNDB.App.SmitteStop.Core.Contracts;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class FileSystemMock : IFileSystem
    {
        public DateTime GetCreationDateUTC(string fileName)
        {
            throw new NotImplementedException();
        }

        public string[] GetFileNamesFromDirectory(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public string JoinPaths(params string?[] paths)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(string filename, byte[] fileContent)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadFile(string path)
        {
            throw new NotImplementedException();
        }

        public string[] GetAllTemporaryFilesFromFolder(string currentZipFilesFolder)
        {
            throw new NotImplementedException();
        }

        public void DeleteFiles(string[] temporaryFileList)
        {
            throw new NotImplementedException();
        }

        public void Rename(string filePath, string newFilePath)
        {
            throw new NotImplementedException();
        }

        public string GetDirectoryNameFromPath(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFileNameFromPath(string filePath)
        {
            throw new NotImplementedException();
        }

        public Stream GetFileStream(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string directoryName)
        {
            if (directoryName.Contains(Constants.DoesExist))
                return true;
            if (directoryName.Contains(Constants.DoesNotExist))
                return false;
            
            throw new ArgumentException(
                $"Invalid argument {directoryName}. Must be one of '{Constants.DoesExist}' or '{Constants.DoesNotExist}'");
        }
    }
}
