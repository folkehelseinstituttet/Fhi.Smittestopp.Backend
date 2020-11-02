using System;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IFileSystem
    {
        DateTime GetCreationDateUTC(string fileName);
        string[] GetFilenamesFromDirectory(string directoryPath);

        string JoinPaths(params string?[] paths);

        void CreateDirectory(string directoryName);
        void WriteAllBytes(string filename, byte[] fileContent);
        void DeleteFile(string filename);
        bool FileExists(string filename);
        byte[] ReadFile(string filename);
    }
}
