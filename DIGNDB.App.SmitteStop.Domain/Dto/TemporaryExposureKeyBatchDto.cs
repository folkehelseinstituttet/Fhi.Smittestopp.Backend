using System.Collections.Generic;
using System.Text.Json;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class TemporaryExposureKeyBatchDto
    {
        public List<TemporaryExposureKeyDto> keys { get; set; }
        public List<string> visitedCountries { get; set; } = new List<string>();
        public List<string> regions { get; set; }
        public string appPackageName { get; set; }
        public string platform { get; set; }
        public bool isSharingAllowed { get; set; }
        public ReportType? ReportType { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}