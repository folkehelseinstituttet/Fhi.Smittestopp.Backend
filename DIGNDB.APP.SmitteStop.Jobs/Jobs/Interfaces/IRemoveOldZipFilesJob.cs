using DIGNDB.APP.SmitteStop.Jobs.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces
{
    public interface IRemoveOldZipFilesJob
    {
        void RemoveOldZipFiles(HangfireConfig hangfireConfig);
    }
}
