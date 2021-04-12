using System;
using System.IO;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IFileSystem
    {
        DateTime GetCreationDateUTC(string fileName);
        string[] GetFileNamesFromDirectory(string directoryPath);

        string JoinPaths(params string?[] paths);

        void CreateDirectory(string path);
        void WriteAllBytes(string filename, byte[] fileContent);
        void DeleteFile(string path);
        bool FileExists(string path);
        byte[] ReadFile(string path);
        string[] GetAllTemporaryFilesFromFolder(string currentZipFilesFolder);
        void DeleteFiles(string[] temporaryFileList);
        void Rename(string filePath, string newFilePath);
        string GetDirectoryNameFromPath(string path);
        string GetFileNameFromPath(string filePath);
        Stream GetFileStream(string path);
    }
}
