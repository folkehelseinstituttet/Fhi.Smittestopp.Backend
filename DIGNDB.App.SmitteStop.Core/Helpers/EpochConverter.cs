using DIGNDB.App.SmitteStop.Core.Contracts;
using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class EpochConverter : IEpochConverter
    {
        public DateTime ConvertFromEpoch(long epochTime)
        {

            var epoch = DateTime.UnixEpoch;
            return epoch.AddSeconds(epochTime);
        }

        public long ConvertToEpoch(DateTime date)
        {
            var epoch = DateTime.UnixEpoch;
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}
