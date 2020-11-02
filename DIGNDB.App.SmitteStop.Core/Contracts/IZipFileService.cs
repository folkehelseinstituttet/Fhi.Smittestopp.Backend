using System;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IZipFileService
    {
        void UpdateZipFiles(DateTime lastCreationDate, DateTime currentDateTime);
    }
}