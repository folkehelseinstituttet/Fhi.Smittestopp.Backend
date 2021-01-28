using DIGNDB.APP.SmitteStop.Jobs.Config;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces
{
    public interface IRemoveOldZipFilesJob
    {
        void RemoveOldZipFiles(HangfireConfig hangfireConfig);
    }
}
