using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public partial class TemporaryExposureKey
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Used to generate broadcasts that are collected on the other devices.
        /// These connect to provide a record of the interaction between two devices.
        /// This information remains on a device until and unless the user tests positive,
        /// at which point the user can choose to share that information with the internet-accessible server.
        /// </summary>
        public byte[] KeyData { get; set; }

        /// <summary>
        /// The time at which the key was generated, in Google standard - 10-minute intervals since UTC epoch but in this class it is just a timestamp (Unix time).
        /// It NEEDS to be converted to 10-minutes intervals (legacy code).
        /// </summary>
        public long RollingStartNumber { get; set; }

        /// <summary>
        /// The number of 10-minute intervals that a key is valid for.
        /// The expiration date for a key can be calculated by adding rollingPeriod to rollingStartNumber.
        /// For keys that are valid for a full day, this value will be equal to 144.
        /// If a key was expired early, the value will be less than 144.
        /// </summary>
        public long RollingPeriod { get; set; }


        public RiskLevel TransmissionRiskLevel { get; set; }


        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// What party has created this key.
        ///
        /// If it was our SmitteStop application (from mobile) we want to indicate here what version of API was used.
        /// If it was not our application then we indicate here what was it, for example the `EU Gateway`.
        /// </summary>
        public KeySource KeySource { get; set; }

        /// <summary>
        /// DK for records from smitte. Other if records is from UE Gateway
        /// </summary>
        public virtual Country Origin { get; set; }

        public int? DaysSinceOnsetOfSymptoms { get; set; }

        public virtual IEnumerable<TemporaryExposureKeyCountry> VisitedCountries { get; set; } = new List<TemporaryExposureKeyCountry>();

        public ReportType ReportType { get; set; }

        public bool SharingConsentGiven { get; set; }
    }
}
