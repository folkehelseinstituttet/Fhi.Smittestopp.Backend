using System;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public class ApplicationStatistics
    {
        public int Id { get; set; }
        public int PositiveResultsLast7Days { get; set; }
        public int SmittestopDownloadsTotal { get; set; }
        public int PositiveTestsResultsTotal { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
