using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public static class DateTimeExtension
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hour)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond,
                DateTimeKind.Utc);
        }
    }
}
