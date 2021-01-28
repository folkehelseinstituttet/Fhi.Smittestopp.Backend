using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class TemporaryExposureKeyBatchDto
    {
        public List<TemporaryExposureKeyDto> keys { get; set; }
        public List<string> visitedCountries { get; set; } = new List<string>();
        public List<string> regions { get; set; }
        public string appPackageName { get; set; }
        public string platform { get; set; }
        public string deviceVerificationPayload { get; set; }
        public bool sharingConsentGiven { get; set; }
    }
}