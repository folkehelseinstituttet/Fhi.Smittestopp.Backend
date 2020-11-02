using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static long ToUnixEpoch(this DateTime dt)
        {
            TimeSpan t = dt - DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            return (long)t.TotalSeconds;
        }
    }
}
