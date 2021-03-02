using System;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class ApplicationStatisticsDto
    {
        public DateTime EntryDate { get; set; }
        public int PositiveResultsLast7Days { get; set; }
        public int SmittestopDownloadsTotal { get; set; }
        public int PositiveTestsResultsTotal { get; set; }
    }
}
