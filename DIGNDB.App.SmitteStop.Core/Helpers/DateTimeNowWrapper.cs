using DIGNDB.App.SmitteStop.Core.Contracts;
using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class DateTimeNowWrapper : IDateTimeNowWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}