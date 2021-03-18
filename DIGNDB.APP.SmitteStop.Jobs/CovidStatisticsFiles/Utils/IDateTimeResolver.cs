using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils
{
    public interface IDateTimeResolver
    {
        void SetDateTime(DateTime dateTime);
        DateTime GetDateXDaysAgo(int X);
        DateTime GetDateToday();
        void SetDateTimeToUtcNow();
        public DateTime GetDateTime();
    }
}