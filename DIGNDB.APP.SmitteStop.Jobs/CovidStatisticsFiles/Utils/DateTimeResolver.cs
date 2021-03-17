using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils
{
    public class DateTimeResolver : IDateTimeResolver
    {
        private DateTime _dateTime;

        public void SetDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        public DateTime GetDateXDaysAgo(int X)
        {
            return _dateTime.Date.AddDays(-X);
        }

        public DateTime GetDateToday()
        {
            return GetDateXDaysAgo(0);
        }

        public void SetDateTimeToUtcNow()
        {
            _dateTime = DateTime.UtcNow;
        }

        public DateTime GetDateTimeNow()
        {
            return _dateTime;
        }
    }
}