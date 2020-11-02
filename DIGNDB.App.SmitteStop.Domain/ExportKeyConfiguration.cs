using System;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class ExportKeyConfiguration
    {
        public TimeSpan PreviousDayFileCaching { get; set; }
        public TimeSpan CurrentDayFileCaching { get; set; }
        public int MaxKeysPerFile { get; set; }
    }
}
