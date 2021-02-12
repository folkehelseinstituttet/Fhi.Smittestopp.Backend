using DIGNDB.App.SmitteStop.Domain;
using System;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IZipFileInfoService
    {
        public int GetNextBatchNumberForGivenDay(string[] zipFilenames, DateTime currentDateTime);
        ZipFileInfo CreateZipFileInfoFromPackageName(string packageName);
        bool CheckIfPackageExists(ZipFileInfo packageInfo, string zipFilesFolder);
        byte[] ReadPackage(ZipFileInfo packageInfo, string zipFilesFolder);
    }
}
