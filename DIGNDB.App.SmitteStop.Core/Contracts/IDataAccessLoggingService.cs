using System.Collections.Generic;
using System.Security.Principal;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IDataAccessLoggingService<TCategoryName>
    {
        void LogDataAccess<TId>(IEnumerable<IIdentifiedEntity<TId>> dataCollection, DataOperation dataOperation, IIdentity identity);
    }
}