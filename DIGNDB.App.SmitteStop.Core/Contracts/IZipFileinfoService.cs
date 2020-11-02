using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain;

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
