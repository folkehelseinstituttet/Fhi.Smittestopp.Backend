using Newtonsoft.Json;
using System;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Models
{
    public class TemporaryExposureKeyDto
    {
        public const int KeyLength = 16;    // Only valid ExposureKey KeyLength in bytes
        public static readonly TimeSpan OneDayTimeSpan = TimeSpan.FromDays(1);
        public TimeSpan rollingDurationSpan { get; private set; } = OneDayTimeSpan;
        public byte[] key { get; set; }
        public DateTime rollingStart { get; set; }
        public string rollingDuration
        {
            get => rollingDurationSpan.ToString();
            set => rollingDurationSpan = TimeSpan.Parse(value);
        }
        public RiskLevel transmissionRiskLevel { get; set; }
        public int? daysSinceOnsetOfSymptoms { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
