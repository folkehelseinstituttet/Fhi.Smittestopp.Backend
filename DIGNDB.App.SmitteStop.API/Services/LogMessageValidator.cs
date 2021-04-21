using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class LogMessageValidator : ILogMessageValidator
    {
        public void ValidateLogMobileMessageDateTimeFormats(LogMessageMobile logMessageMobile)
        {
            IList<string> elements = logMessageMobile.GetDateTimeFormatsValueToVerify();
            foreach (var element in elements)
            {
                if (!IsValidDateTimeFormat(element))
                    throw new JsonException("Invalid datetime format");
            }
        }

        public void ValidateLogMobileMessagePatterns(LogMessageMobile logMessageMobile, IDictionary<string, string> patternsDictionary)
        {
            IDictionary<string, KeyValuePair<string, string>> elements = logMessageMobile.GetPatternsValueToVerify(patternsDictionary);
            foreach (var keyValuePair in elements)
            {
                if (!IsValidRegularExpression(keyValuePair.Value))
                    throw new JsonException("Invalid pattern");
            }
        }

        private bool IsValidRegularExpression(KeyValuePair<string, string> keyValuePair)
        {
            Regex r = new Regex(string.Format(@"{0}", keyValuePair.Key));
            return r.IsMatch(keyValuePair.Value);
        }

        private bool IsValidDateTimeFormat(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                TimeSpan timeSpan = (DateTime.UtcNow - result).Duration();
                double years = timeSpan.TotalDays / 365.25;
                if (years > 2)
                    return false;
                return true;
            }
            return false;
        }

        public void SanitizeAndShortenTextFields(LogMessageMobile lm, int maxLength)
        {
            SanitizeAndShortenField(lm.api, s => lm.api = s, maxLength);
            SanitizeAndShortenField(lm.exceptionType, s => lm.exceptionType = s, maxLength);
            SanitizeAndShortenField(lm.exceptionMessage, s => lm.exceptionMessage = s, maxLength);
            SanitizeAndShortenField(lm.exceptionStackTrace, s => lm.exceptionStackTrace = s, maxLength);
            SanitizeAndShortenField(lm.innerExceptionType, s => lm.innerExceptionType = s, maxLength);
            SanitizeAndShortenField(lm.innerExceptionMessage, s => lm.innerExceptionMessage = s, maxLength);
            SanitizeAndShortenField(lm.innerExceptionStackTrace, s => lm.innerExceptionStackTrace = s, maxLength);
            SanitizeAndShortenField(lm.correlationId, s => lm.correlationId = s, maxLength);
            SanitizeAndShortenField(lm.severity, s => lm.severity = s, maxLength);
            SanitizeAndShortenField(lm.description, s => lm.description = s, maxLength);
            SanitizeAndShortenField(lm.buildVersion, s => lm.buildVersion = s, maxLength);
            SanitizeAndShortenField(lm.buildNumber, s => lm.buildNumber = s, maxLength);
            SanitizeAndShortenField(lm.deviceType, s => lm.deviceType = s, maxLength);
            SanitizeAndShortenField(lm.deviceDescription, s => lm.deviceDescription = s, maxLength);
            SanitizeAndShortenField(lm.deviceOSVersion, s => lm.deviceOSVersion = s, maxLength);
            SanitizeAndShortenField(lm.apiErrorMessage, s => lm.apiErrorMessage = s, maxLength);
            SanitizeAndShortenField(lm.additionalInfo, s => lm.additionalInfo = s, maxLength);
        }

        public void SanitizeAndShortenField(string value, Action<string> setFieldOnLogMessage, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(value)) return;
            var modifiedField = Sanitizer.GetSafeHtmlFragment(value.Trim());
            bool valueHasChanged = (!modifiedField.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (modifiedField.Length > maxLength)
            {
                modifiedField = modifiedField.Substring(0,maxLength);
                valueHasChanged = true;
            }
            if (valueHasChanged)
            {
                setFieldOnLogMessage(modifiedField);
            }
        }


    }
}
