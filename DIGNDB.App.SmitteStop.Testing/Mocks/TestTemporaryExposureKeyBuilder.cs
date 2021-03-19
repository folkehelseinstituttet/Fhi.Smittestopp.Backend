using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class TestTemporaryExposureKeyBuilder
    {
        private readonly TemporaryExposureKey _prototype;
        private IList<Country> _visitedCountries = new List<Country>(0);

        public static class Default
        {
            public static byte[] KeyData = { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 };
            public static int DaysSinceOnsetOfSymptoms = 4;
            public static ReportType ReportType = ReportType.SELF_REPORT;
            public static long RollingPeriod = 144;
            public static long RollingStartNumberDaysAgo = 1;
            public static RiskLevel TransmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH;
        }

        private static long CurrentUnixTimestampOffsetByDays(long numberOfDays)
        {
            var creationDate = DateTime.Now.AddDays(numberOfDays);
            return ConvertDateToUnixTime(creationDate);
        }

        private static long ConvertDateToUnixTime(DateTime date)
        {
            var epoch = DateTime.UnixEpoch;
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static TestTemporaryExposureKeyBuilder CreateDefault(Country origin)
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var rollingStartNumber = CurrentUnixTimestampOffsetByDays(-Default.RollingStartNumberDaysAgo);

            var defaultPrototype = new TemporaryExposureKey()
            {
                KeyData = Default.KeyData,
                CreatedOn = yesterday,
                DaysSinceOnsetOfSymptoms = Default.DaysSinceOnsetOfSymptoms,
                Origin = origin,
                ReportType = Default.ReportType,
                KeySource = Domain.Enums.KeySource.SmitteStopApiVersion2,
                RollingPeriod = Default.RollingPeriod,
                RollingStartNumber = rollingStartNumber,
                TransmissionRiskLevel = Default.TransmissionRiskLevel,
                VisitedCountries = new List<TemporaryExposureKeyCountry>(),
                SharingConsentGiven = true
            };
            return new TestTemporaryExposureKeyBuilder(defaultPrototype);
        }

        public IList<TemporaryExposureKey> BuildNormalized(IList<string> keyData)
        {
            return Build(keyData, true);
        }

        public TestTemporaryExposureKeyBuilder Copy()
        {
            return new TestTemporaryExposureKeyBuilder(Build()); ;
        }

        private TestTemporaryExposureKeyBuilder(TemporaryExposureKey prototype)
        {
            _prototype = prototype;
        }

        public IList<TemporaryExposureKey> Build(IList<string> keyData)
        {
            return keyData.Select(keyDataStr => Encoding.ASCII.GetBytes(keyDataStr))
                .Select(keyData => Build(keyData))
                .ToList();
        }

        public IList<TemporaryExposureKey> Build(IList<byte[]> keyData)
        {
            return keyData.Select(keyData => Build(keyData))
                .ToList();
        }

        public TemporaryExposureKey Build()
        {
            return Build(_prototype.KeyData);
        }

        public TemporaryExposureKey Build(byte[] keyData)
        {
            var key = new TemporaryExposureKey()
            {
                CreatedOn = _prototype.CreatedOn,
                KeyData = (byte[])keyData.Clone(),
                KeySource = _prototype.KeySource,
                Origin = _prototype.Origin,
                ReportType = _prototype.ReportType,
                RollingPeriod = _prototype.RollingPeriod,
                RollingStartNumber = _prototype.RollingStartNumber,
                TransmissionRiskLevel = _prototype.TransmissionRiskLevel,
                DaysSinceOnsetOfSymptoms = _prototype.DaysSinceOnsetOfSymptoms,
                SharingConsentGiven = _prototype.SharingConsentGiven
            };

            var allVisitedCountries = _prototype.VisitedCountries.Select(i => i.Country).ToList();
            allVisitedCountries.AddRange(_visitedCountries);

            key.VisitedCountries = allVisitedCountries.Select(country =>
            {
                return new TemporaryExposureKeyCountry() { Country = country, TemporaryExposureKey = key };
            }).ToList();

            return key;
        }

        private IList<TemporaryExposureKey> Build(IList<string> keyData, bool force16BytesLength)
        {
            return keyData.Select(keyDataStr => Encoding.ASCII.GetBytes(keyDataStr))
                .Select(keyDataBytes => force16BytesLength ? NormalizeKeyDataTo16BytesOrThrow(keyDataBytes) : keyDataBytes)
                .Select(keyData => Build(keyData))
                .ToList();
        }

        private byte[] NormalizeKeyDataTo16BytesOrThrow(byte[] originalBytes)
        {
            byte[] keyDate = new byte[16];
            if (originalBytes.Length > 16)
            {
                throw new ArgumentException("KeyData has Length > 16!");
            }
            for (int i = 0; i < Default.KeyData.Length; ++i)
            {
                keyDate[i] = i >= originalBytes.Length ? Default.KeyData[i] : originalBytes[i];
            }
            return keyDate;
        }

        #region setters
        public TestTemporaryExposureKeyBuilder SetCreatedOn(DateTime date)
        {
            _prototype.CreatedOn = date;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetKeyData(byte[] keyData)
        {
            _prototype.KeyData = keyData;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetKeySource(KeySource keySource)
        {
            _prototype.KeySource = keySource;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetOrigin(Country origin)
        {
            _prototype.Origin = origin;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetVisitedCountries(IEnumerable<Country> countries)
        {
            _prototype.VisitedCountries = new List<TemporaryExposureKeyCountry>();
            _visitedCountries = new List<Country>(countries);
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetReportType(ReportType reportType)
        {
            _prototype.ReportType = reportType;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetRollingPeriod(long rollingPeriod)
        {
            _prototype.RollingPeriod = rollingPeriod;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetRollingStartNumber(long rollingStartNumber)
        {
            _prototype.RollingStartNumber = rollingStartNumber;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetRollingStartNumber(DateTime rollingStartNumber)
        {
            _prototype.RollingStartNumber = ConvertDateToUnixTime(rollingStartNumber);
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetTransmissionRiskLevel(RiskLevel transmissionRiskLevel)
        {
            _prototype.TransmissionRiskLevel = transmissionRiskLevel;
            return this;
        }

        public TestTemporaryExposureKeyBuilder SetDaysSinceOnsetOfSymptoms(int? daysSinceOnsetOfSymptoms)
        {
            _prototype.DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;
            return this;
        }
        #endregion
    }
}
