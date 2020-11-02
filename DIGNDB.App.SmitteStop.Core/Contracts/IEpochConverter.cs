using System;
using System.Collections.Generic;
using System.Text;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IEpochConverter
    {
        DateTime ConvertFromEpoch(long epochTime);
        long ConvertToEpoch(DateTime date);

    }
}
