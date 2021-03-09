using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Enums;
using System;
using System.IO;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Dto
{
    public class CsvFileDto : IDisposable
    {
        public CovidStatisticsFileName Name { get; set; }
        public Stream File { get; set; }

        public void Dispose()
        {
            File?.Dispose();
        }
    }
}