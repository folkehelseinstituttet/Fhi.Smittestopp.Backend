namespace FederationGatewayApi.Models
{
    public enum SymptomStatus
    {
        Unknown = 0,
        Symptomatic = 1,
        Asymptomatic = 3
    }

    public enum DateType
    {
        Unknown = 0,
        PreciseDate = 1,
        Range = 2,
    }

    public class DaysSinceOnsetOfSymptomsResults
    {
        public bool IsValid { get; }
        public int Offset { get; set; }
        public SymptomStatus SymptomStatus { get; set; }
        public int? IntervalDuration { get; set; }
        public DateType DateType { get; set; }
        public int? DaysSinceOnsetOfSymptoms { get; set; }
        public DaysSinceOnsetOfSymptomsResults(bool isValid)
        {
            IsValid = isValid;
        }

    }
}
