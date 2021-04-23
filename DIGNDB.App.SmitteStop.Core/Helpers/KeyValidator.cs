using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class KeyValidator : IKeyValidator
    {
        #region Consts and properties
        public IEpochConverter _epochConverter { get; set; }

        private const int KeySize = 16;
        private const ReportType RequiredReportType = ReportType.CONFIRMED_TEST;
        private const string Norway = "NO";
        private const int NorwayId = 29;
        private const bool Enabled = true;
        private const bool Valid = true;
        private const long RollingPeriodMax = 144;
        private const long RollingPeriodMin = 1;

        private const int ValidRollingStartNumberOffset = 14;

        public const int DaysSinceOnsetOfSymptomsValidRangeMin = -14;
        public const int DaysSinceOnsetOfSymptomsValidRangeMax = 14;

        public const int DaysSinceOnsetOfSymptomsInvalidRangeMin = 2986;
        public const int DaysSinceOnsetOfSymptomsInvalidRangeMax = 3014;
        public const string ErrorPrefix = "-";

        #endregion

        public KeyValidator(IEpochConverter epochConverter)
        {
            _epochConverter = epochConverter;
        }

        public bool ValidateKeyAPI(TemporaryExposureKey exposureKey, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!ValidateKeyData(exposureKey.KeyData))
            {
                errorMessage += $"{ErrorPrefix} The KeyData is not a 16byte field. It has more or less.";
                return !Valid;
            }
            if (!ValidateRollingPeriod(exposureKey.RollingPeriod))
            {
                errorMessage += $"{ErrorPrefix} The RollingPeriod date should be in range of 10 minutes to 24 hours. RollingPeriod: {exposureKey.RollingPeriod}";
                return !Valid;
            }
            if (!ValidateRollingStartNumber(exposureKey.RollingStartNumber))
            {
                errorMessage += $"{ErrorPrefix} The RollingStartNumber date is invalid [in the future or older than 14 days]. RollingStartNumber: {exposureKey.RollingStartNumber}";
                return !Valid;
            }
            return Valid;
        }

        public bool ValidateKeyGateway(TemporaryExposureKey exposureKey, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!ValidateKeyAPI(exposureKey, out var apiValidationErrorMessage))
            {
                errorMessage += apiValidationErrorMessage;
                return !Valid;
            }
            if (!ValidateKeyReportType(exposureKey.ReportType))
            {
                errorMessage += $"{ErrorPrefix} ReportType is not valid for this key - {exposureKey.ReportType}";
                return !Valid;
            }
            if (!ValidateOrigin(exposureKey.Origin))
            {
                errorMessage += $"{ErrorPrefix} The Origin field is invalid. Code {exposureKey.Origin?.Code}, PullingFromGatewayEnabled= {exposureKey.Origin?.PullingFromGatewayEnabled} ";
                return !Valid;
            }
            if (!ValidateDaysSinceOnsetOfSymptoms(exposureKey.DaysSinceOnsetOfSymptoms))
            {
                errorMessage += $"{ErrorPrefix} The DaysSinceOnesetOfSymptoms is invalid. It should be in range of {DaysSinceOnsetOfSymptomsInvalidRangeMin} and {DaysSinceOnsetOfSymptomsInvalidRangeMax}.";
                return !Valid;
            }

            return Valid;
        }

        private bool ValidateDaysSinceOnsetOfSymptoms(int? exposureKeyDaysSinceOnsetOfSymptoms)
        {
            return !(exposureKeyDaysSinceOnsetOfSymptoms >= DaysSinceOnsetOfSymptomsInvalidRangeMin) ||
                   !(exposureKeyDaysSinceOnsetOfSymptoms <= DaysSinceOnsetOfSymptomsInvalidRangeMax);
        }


        #region Private methods

        private bool ValidateRollingPeriod(long rollingPeriod)
        {
            return InRange(rollingPeriod, RollingPeriodMin, RollingPeriodMax);
        }

        private bool ValidateRollingStartNumber(long rollingStartNumber)
        {

            DateTime rollingStartDate = _epochConverter.ConvertFromEpoch(rollingStartNumber).Date;

            var dateDaysDifference = (DateTime.UtcNow.Date - rollingStartDate).TotalDays;

            return InRange(dateDaysDifference, 0, ValidRollingStartNumberOffset);
        }

        private bool ValidateOrigin(Country origin)
        {
            return origin.PullingFromGatewayEnabled == Enabled &&
                   origin.Code != Norway &&
                   origin.Id != NorwayId;
        }

        private bool ValidateKeyReportType(ReportType reportType)
        {
            return reportType == RequiredReportType;
        }

        private bool ValidateKeyData(byte[] keyData)
        {
            return keyData.Length == KeySize;
        }

        private bool InRange(long value, long minimum, long maximum)
        {
            return value >= minimum && value <= maximum;
        }
        private bool InRange(double value, double minimum, double maximum)
        {
            return value >= minimum && value <= maximum;
        }
        #endregion
    }
}
