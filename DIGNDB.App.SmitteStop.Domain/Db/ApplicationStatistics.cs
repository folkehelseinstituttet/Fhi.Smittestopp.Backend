using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    [JsonConverter(typeof(ApplicationStatisticsRequiredPropertyConverterForAttributeRegistration))]

    public class ApplicationStatistics
    {
        public int Id { get; set; }
        public int PositiveResultsLast7Days { get; set; }
        public int SmittestopDownloadsTotal { get; set; }
        public int PositiveTestsResultsTotal { get; set; }
        public DateTime EntryDate { get; set; }
    }

    public class ApplicationStatisticsWithoutRequiredPropertyConverterAttribute : ApplicationStatistics
    {
    }

    public class ApplicationStatisticsRequiredPropertyConverterForAttributeRegistration :
        JsonConverter<ApplicationStatistics>
    {
        public override ApplicationStatistics Read(
            ref Utf8JsonReader reader,
            Type type,
            JsonSerializerOptions options)
        {
            // OK to pass in options when recursively calling Deserialize.
            ApplicationStatistics applicationStatistics =
                JsonSerializer.Deserialize<ApplicationStatisticsWithoutRequiredPropertyConverterAttribute>(
                    ref reader,
                    options);

            // Check for required fields set by values in JSON.
            var a = applicationStatistics.PositiveResultsLast7Days == default;
            var b = applicationStatistics.PositiveTestsResultsTotal == default;
            var c = applicationStatistics.SmittestopDownloadsTotal == default;
            return a && b && c
                ? throw new JsonException("Required property not received in the JSON")
                : applicationStatistics;
        }

        public override void Write(
            Utf8JsonWriter writer,
            ApplicationStatistics applicationStatistics,
            JsonSerializerOptions options)
        {
            var ApplicationStatisticsWithoutConverterAttributeOnClass =
                new ApplicationStatisticsWithoutRequiredPropertyConverterAttribute
                {
                    EntryDate = applicationStatistics.EntryDate,
                    PositiveResultsLast7Days = applicationStatistics.PositiveResultsLast7Days,
                    PositiveTestsResultsTotal = applicationStatistics.PositiveTestsResultsTotal,
                    SmittestopDownloadsTotal = applicationStatistics.SmittestopDownloadsTotal
                };

            // OK to pass in options when recursively calling Serialize.
            JsonSerializer.Serialize(
                writer,
                ApplicationStatisticsWithoutConverterAttributeOnClass,
                options);
        }
    }
}
