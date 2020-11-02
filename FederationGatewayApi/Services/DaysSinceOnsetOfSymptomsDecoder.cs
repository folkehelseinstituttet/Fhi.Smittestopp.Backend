using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using System;

namespace FederationGatewayApi.Services
{
    public class DaysSinceOnsetOfSymptomsDecoder : IDaysSinceOnsetOfSymptomsDecoder
    {
        public DaysSinceOnsetOfSymptomsResults Decode(int dsosInGatewayFormat)
        {
            int offset = 0;
            SymptomStatus symptomStatus = SymptomStatus.Unknown;
            DateType dateType = DateType.Unknown;
            int? intervalDuration = null;
            int? dsos; // null for unknown
            var isValid = true;

            if (dsosInGatewayFormat >= -14 && dsosInGatewayFormat <= 21)
            {
                offset = 0;
                symptomStatus = SymptomStatus.Symptomatic;
                dateType = DateType.PreciseDate;
            }
            else if (dsosInGatewayFormat < 1950)
            {
                intervalDuration = (int)Math.Round(dsosInGatewayFormat / 100d);
                offset = intervalDuration.Value * 100;
                symptomStatus = SymptomStatus.Symptomatic;

                dateType = DateType.Range;
            }
            else if (dsosInGatewayFormat >= 1986 && dsosInGatewayFormat <= 2000)
            {
                symptomStatus = SymptomStatus.Symptomatic;
                dateType = DateType.Unknown;

                offset = 2000;
            }
            else if (dsosInGatewayFormat >= 2986 && dsosInGatewayFormat <= 3000)
            {
                symptomStatus = SymptomStatus.Asymptomatic;
                dateType = DateType.Unknown;

                offset = 3000;
            }
            else if (dsosInGatewayFormat >= 3986 && dsosInGatewayFormat <= 4000)
            {
                symptomStatus = SymptomStatus.Unknown;//unknown (no information);
                dateType = DateType.Unknown;

                offset = 4000;
            }
            else
            {
                isValid = false;
            }

            dsos = dsosInGatewayFormat - offset;
            if (dsos < -14 || dsos > 21)
            {
                isValid = false;
            }

            return new DaysSinceOnsetOfSymptomsResults(isValid)
            {
                Offset = offset,
                SymptomStatus = symptomStatus,
                IntervalDuration = intervalDuration,
                DateType = dateType,
                DaysSinceOnsetOfSymptoms = dsos
            };
        }
    }
}
