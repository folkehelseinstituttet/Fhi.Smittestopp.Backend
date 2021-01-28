using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class TemporaryExposureKeyDto
    {
        public const int KeyLength = 16; // Only valid ExposureKey KeyLength in bytes
        public static readonly TimeSpan OneDayTimeSpan = TimeSpan.FromDays(1);
        public TimeSpan rollingDurationSpan { get; private set; } = OneDayTimeSpan;
        public byte[] key { get; set; }
        [JsonConverter(typeof(UTCMidnightJsonConverter))]
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
            return JsonSerializer.Serialize(this);
        }

        private class UTCMidnightJsonConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                return DateTimeOffset.Parse(reader.GetString()).UtcDateTime.ToUniversalTime().Date;
            }

            public override void Write(
                Utf8JsonWriter writer,
                DateTime dateTimeValue,
                JsonSerializerOptions options)
            {
                writer.WriteStringValue(dateTimeValue.ToString());
            }
        }
    }
}
