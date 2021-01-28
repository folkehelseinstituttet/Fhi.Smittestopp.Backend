using DIGNDB.App.SmitteStop.Domain.Dto;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface ILogMessageValidator
    {
        void ValidateLogMobileMessagePatterns(LogMessageMobile logMessageMobile,
            IDictionary<string, string> patternsDictionary);
        void ValidateLogMobileMessageDateTimeFormats(LogMessageMobile logMessageMobile);
        void SanitizeAndShortenTextFields(LogMessageMobile logMessageMobile, int maxLength = 500);
    }
}
