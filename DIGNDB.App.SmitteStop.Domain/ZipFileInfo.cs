using System;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class ZipFileInfo
    {
        public DateTime PackageDate { get; set; }
        public int BatchNumber { get; set; }
        public string Origin { get; set; }
        public string FileName
        {
            get
            {
                return ($"{PackageDate:yyyy-MM-dd}_{BatchNumber}_{Origin}.zip");
            }
        }
    }
}
