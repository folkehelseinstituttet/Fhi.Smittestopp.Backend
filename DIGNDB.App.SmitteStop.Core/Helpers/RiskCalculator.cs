using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class RiskCalculator : IRiskCalculator
    {

        public RiskLevel CalculateRiskLevel(TemporaryExposureKey exposureKey)
        {
            return exposureKey.DaysSinceOnsetOfSymptoms != null
                ? CalculateRiskBasedOnDays(exposureKey.DaysSinceOnsetOfSymptoms)
                : RiskLevel.RISK_LEVEL_HIGHEST;
        }


        private RiskLevel CalculateRiskBasedOnDays(int? daysSinceOnsetOfSymptoms)
        {
            RiskLevel riskLevel = RiskLevel.RISK_LEVEL_INVALID;

            for (int i = DaysSinceOnsetForTransmissionRiskCalculation.Length - 1; i >= 0; i--)
            {
                if (daysSinceOnsetOfSymptoms >= DaysSinceOnsetForTransmissionRiskCalculation[i].Item1
                    && daysSinceOnsetOfSymptoms <= DaysSinceOnsetForTransmissionRiskCalculation[i].Item2)
                {
                    riskLevel = (RiskLevel)i + 1;
                    break;
                }
            }
            return riskLevel;
        }

        private static readonly Tuple<int, int>[] DaysSinceOnsetForTransmissionRiskCalculation =
        {
            // "-" represents days BEFORE the symptom onset, positive number is for days AFTER symptom onset
            Tuple.Create(int.MinValue, int.MaxValue), // For array index 1 (Represents Lowest risk, should be set for all the keys)
            Tuple.Create(-3, -3),                     // For array index 2
            Tuple.Create(-2, -2),                     // For array index 3
            Tuple.Create(-1, 2),                      // For array index 4
            Tuple.Create(3, 6),                       // For array index 5
            Tuple.Create(7, 8),                       // For array index 6
            Tuple.Create(9, 10),                      // For array index 7
            Tuple.Create(11, 12)                      // For array index 8
        };

    }
}
