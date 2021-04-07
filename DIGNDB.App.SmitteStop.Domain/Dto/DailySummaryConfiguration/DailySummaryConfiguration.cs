using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Domain.Dto.DailySummaryConfiguration
{
    public class DailySummaryConfiguration
    {
        public int[] AttenuationThresholds { get; set; }

        public Dictionary<DistanceEstimate, double> AttenuationWeights { get; set; }

        public int DaysSinceLastExposureThreshold { get; set; }

        public Dictionary<string, int> DaysSinceOnsetInfectiousness { get; set; }

        public Infectiousness DefaultInfectiousness { get; set; }

        public ReportType DefaultReportType { get; set; }

        public Dictionary<Infectiousness, double> InfectiousnessWeights { get; set; }

        public Dictionary<ReportType, double> ReportTypeWeights { get; set; }
    }

    public enum CalibrationConfidence
    {
        Lowest,
        Low,
        Medium,
        High
    }

    public enum DistanceEstimate
    {
        Immediate,
        Near,
        Medium,
        Other,
    }

    public enum Infectiousness
    {
        None,
        Standard,
        High,
    }

    public enum ReportType
    {
        Unknown,
        ConfirmedTest,
        ConfirmedClinicalDiagnosis,
        SelfReported,
        Recursive,
        Revoked,
    }
}
