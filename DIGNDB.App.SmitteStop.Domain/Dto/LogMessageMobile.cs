using System.Collections.Generic;
using System.Text;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class LogMessageMobile
    {

        public int apiVersion { get; set; }
        public string severity { get; set; }
        public string description { get; set; }
        public string buildVersion { get; set; }
        public string buildNumber { get; set; }
        public string deviceType { get; set; }
        public string deviceDescription { get; set; }
        public string deviceOSVersion { get; set; }

        public string exceptionType { get; set; }
        public string exceptionMessage { get; set; }
        public string exceptionStackTrace { get; set; }
        public string innerExceptionType { get; set; }
        public string innerExceptionMessage { get; set; }
        public string innerExceptionStackTrace { get; set; }

        //Used for the authentication/submission flow
        public string correlationId { get; set; }

        public string api { get; set; }
        public int? apiErrorCode { get; set; }
        public string apiErrorMessage { get; set; }
        public string reportedTime { get; set; }
        public string additionalInfo { get; set; }

        public IDictionary<string, KeyValuePair<string, string>> GetPatternsValueToVerify(IDictionary<string, string> dictionary)
        {
            Dictionary<string, KeyValuePair<string, string>> elements = new Dictionary<string, KeyValuePair<string, string>>();
            elements.Add("severity", new KeyValuePair<string, string>(dictionary["severityRegex"], severity));
            elements.Add("apiVersion", new KeyValuePair<string, string>(dictionary["positiveNumbersRegex"], apiVersion.ToString()));
            elements.Add("buildVersion", new KeyValuePair<string, string>(dictionary["buildVersionRegex"], buildVersion));
            elements.Add("buildNumber", new KeyValuePair<string, string>(dictionary["positiveNumbersRegex"], buildNumber));
            elements.Add("deviceType", new KeyValuePair<string, string>(dictionary["operationSystemRegex"], deviceType));
            elements.Add("deviceOSVersion", new KeyValuePair<string, string>(dictionary["deviceOSVersionRegex"], deviceOSVersion));
            return elements;
        }

        public IList<string> GetDateTimeFormatsValueToVerify()
        {
            IList<string> elements = new List<string>();
            elements.Add(reportedTime);
            return elements;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"reported time: {reportedTime}");
            sb.AppendLine($"description: {description}");
            sb.AppendLine($"apiVersion: {apiVersion}");
            sb.AppendLine($"buildVersion:{buildVersion}");
            sb.AppendLine($"buildNumber:{buildNumber}");
            sb.AppendLine($"deviceOSVersion:{deviceOSVersion}");
            sb.AppendLine($"deviceType:{deviceType}");
            sb.AppendLine($"deviceDescription:{deviceDescription}");
            sb.AppendLine($"exceptionType:{exceptionType}");
            sb.AppendLine($"exceptionMessage:{exceptionMessage}");
            sb.AppendLine($"exceptionStackTrace:{exceptionStackTrace}");
            sb.AppendLine($"innerExceptionType:{innerExceptionType}");
            sb.AppendLine($"innerExceptionMessage:{innerExceptionMessage}");
            sb.AppendLine($"innerExceptionStackTrace:{innerExceptionStackTrace}");
            sb.AppendLine($"correlationId:{correlationId}");
            sb.AppendLine($"api:{api}");
            sb.AppendLine($"apiErrorCode:{apiErrorCode}");
            sb.AppendLine($"apiErrorMessage:{apiErrorMessage}");
            sb.AppendLine($"additionalInfo:{additionalInfo}");
            return sb.ToString();
        }
    }
}
