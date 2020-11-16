using System;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IEpochConverter
    {
        DateTime ConvertFromEpoch(long epochTime);
        long ConvertToEpoch(DateTime date);

    }
}
