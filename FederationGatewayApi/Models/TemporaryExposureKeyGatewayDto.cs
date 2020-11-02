using System.Collections.Generic;

namespace FederationGatewayApi.Models
{
    /// <summary>
    /// https://github.com/eu-federation-gateway-service/efgs-federation-gateway
    /// https://developers.google.com/android/exposure-notifications/exposure-notifications-api#data-structures
    /// </summary>
    public class TemporaryExposureKeyGatewayDto
    {
        public byte[] KeyData { get; set; }
        
        // The time at which the key was generated, in 10-minute intervals since UTC epoch. This time will align to UTC midnight.
        public uint RollingStartIntervalNumber { get; set; }

        /*
        The number of 10-minute intervals that a key is valid for.
        The expiration date for a key can be calculated by adding rollingPeriod to rollingStartNumber.
        For keys that are valid for a full day, this value will be equal to 144. If a key was expired early, the value will be less than 144.
        */
        public uint RollingPeriod { get; set; }

        public int TransmissionRiskLevel { get; set; }

        // Each country (e.g., DE) has 2 bytes. UTF-8 encoding.Ascending  Ascending alphabetic order(e.g., DE, NL, UK).
        public IList<string> VisitedCountries { get; set; }

        public string Origin { get; set; }

        public string ReportType { get; set; }

        public int DaysSinceOnsetOfSymptoms { get; set; }
    }
}
