using System;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IDateTimeNowWrapper
    {
        DateTime UtcNow { get; }
    }
}