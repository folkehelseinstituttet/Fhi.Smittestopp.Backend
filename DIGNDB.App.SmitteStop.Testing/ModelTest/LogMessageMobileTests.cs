using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Text;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class LogMessageMobileTests
    {
        LogMessageMobile _log;

        [SetUp]
        public void init()
        {
            _log = new LogMessageMobile();
        }

        [Test]
        public void LogMessageMobile_CallToStringMethod_ShouldGetCorrectLog()
        {
            var reportedTime = "1:00:00";
            var description = "description";
            var apiVersion = 1;
            var buildVersion = "v1.0.1";
            var buildNumber = "1.0.1";
            var deviceOSVersion = "iOS12";
            var deviceType = "phone";
            var deviceDescription = ".";
            var exceptionType = "no error";
            var exceptionMessage = "";
            var exceptionStackTrace = "123abweidj";
            var innerExceptionType = "123[.]";
            var innerExceptionMessage = "test desciption";
            var innerExceptionStackTrace = "123123";
            var api = "pull keys";
            var apiErrorCode = 204;
            var apiErrorMessage = "no content";
            var additionalInfo = "";
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
            sb.AppendLine($"api:{api}");
            sb.AppendLine($"apiErrorCode:{apiErrorCode}");
            sb.AppendLine($"apiErrorMessage:{apiErrorMessage}");
            sb.AppendLine($"additionalInfo:{additionalInfo}");
            var expect = sb.ToString();

            _log.reportedTime = reportedTime;
            _log.description = description;
            _log.apiVersion = apiVersion;
            _log.buildVersion = buildVersion;
            _log.buildNumber = buildNumber;
            _log.deviceOSVersion = deviceOSVersion;
            _log.deviceType = deviceType;
            _log.deviceDescription = deviceDescription;
            _log.exceptionType = exceptionType;
            _log.exceptionMessage = exceptionMessage;
            _log.exceptionStackTrace = exceptionStackTrace;
            _log.innerExceptionType = innerExceptionType;
            _log.innerExceptionMessage = innerExceptionMessage;
            _log.innerExceptionStackTrace = innerExceptionStackTrace;
            _log.api = api;
            _log.apiErrorCode = apiErrorCode;
            _log.apiErrorMessage = apiErrorMessage;
            _log.additionalInfo = additionalInfo;

            Assert.AreEqual(expect, _log.ToString());
        }

        [Test]
        public void LogMessageMobile_GetDateTimeFormatsValueToVerify_ShouldReturnListString()
        {
            var reportTime = "12:00:00";
            _log.reportedTime = reportTime;
            var formatToVerify = _log.GetDateTimeFormatsValueToVerify();
            Assert.IsInstanceOf<List<string>>(formatToVerify);
            Assert.IsTrue(formatToVerify.Count > 0);
            Assert.AreEqual(reportTime, formatToVerify[0]);
        }

        [Test]
        public void LogMessageMobile_GetPatternsValueToVerify_ShouldReturnListOfLogValiationModel()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("severityRegex", "low");
            dictionary.Add("buildVersionRegex", "v1.0.1");
            dictionary.Add("operationSystemRegex", "ios");
            dictionary.Add("deviceOSVersionRegex", "iOS12");
            dictionary.Add("positiveNumbersRegex", "12");
            var pattern = _log.GetPatternsValueToVerify(dictionary);
            Assert.IsInstanceOf<Dictionary<string, KeyValuePair<string, string>>>(pattern);
        }
    }
}
