using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Utils
{
    public interface IDateTimeResolver
    {
        void SetDateTime(DateTime dateTime);
        DateTime GetDateXDaysAgo(int X);
        DateTime GetDateToday();
        void SetDateTimeToUtcNow();
        public DateTime GetDateTimeNow();
    }
}