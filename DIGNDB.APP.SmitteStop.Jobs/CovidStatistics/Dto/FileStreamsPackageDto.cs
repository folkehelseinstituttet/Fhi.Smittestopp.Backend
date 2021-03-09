using System;
using System.Collections.Generic;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Dto
{
    public class FileStreamsPackageDto : IDisposable
    {
        public List<CsvFileDto> Files { get; set; }

        public FileStreamsPackageDto()
        {
            Files = new List<CsvFileDto>();
        }
        public void Dispose()
        {
            foreach (var file in Files)
            {
                file.Dispose();
            }
        }
    }
}