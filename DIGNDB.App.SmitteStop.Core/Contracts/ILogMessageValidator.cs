using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;

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
