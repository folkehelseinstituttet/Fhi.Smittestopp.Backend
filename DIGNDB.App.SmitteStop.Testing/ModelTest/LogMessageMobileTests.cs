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
        public void LogMessageMobile_SetApiVersion_ShouldGetCorrectValue()
        {
            var expect = 1;
            _log.apiVersion = expect;
            Assert.AreEqual(expect, _log.apiVersion);
        }

        [Test]
        public void LogMessageMobile_SetSeverity_ShouldGetCorrectValue()
        {
            var expect = "high";
            _log.severity = expect;
            Assert.AreEqual(expect, _log.severity);
        }

        [Test]
        public void LogMessageMobile_SetDescription_ShouldGetCorrectValue()
        {
            var expect = "test description";
            _log.description = expect;
            Assert.AreEqual(expect, _log.description);
        }

        [Test]
        public void LogMessageMobile_SetBuildVersion_ShouldGetCorrectValue()
        {
            var expect = "v1";
            _log.buildVersion = expect;
            Assert.AreEqual(expect, _log.buildVersion);
        }

        [Test]
        public void LogMessageMobile_SetBuildNumber_ShouldGetCorrectValue()
        {
            var expect = "1.0";
            _log.buildNumber = expect;
            Assert.AreEqual(expect, _log.buildNumber);
        }

        [Test]
        public void LogMessageMobile_SetDeviceType_ShouldGetCorrectValue()
        {
            var expect = "mobile";
            _log.deviceType = expect;
            Assert.AreEqual(expect, _log.deviceType);
        }

        [Test]
        public void LogMessageMobile_SetDeviceDescription_ShouldGetCorrectValue()
        {
            var expect = "abc";
            _log.deviceDescription = expect;
            Assert.AreEqual(expect, _log.deviceDescription);
        }

        [Test]
        public void LogMessageMobile_SetOSVersion_ShouldGetCorrectValue()
        {
            var expect = "iOS12";
            _log.deviceOSVersion = expect;
            Assert.AreEqual(expect, _log.deviceOSVersion);
        }

        [Test]
        public void LogMessageMobile_SetExceptionType_ShouldGetCorrectValue()
        {
            var expect = "server error";
            _log.exceptionType = expect;
            Assert.AreEqual(expect, _log.exceptionType);
        }

        [Test]
        public void LogMessageMobile_SetExceptionMessage_ShouldGetCorrectValue()
        {
            var expect = ".";
            _log.exceptionMessage = expect;
            Assert.AreEqual(expect, _log.exceptionMessage);
        }

        [Test]
        public void LogMessageMobile_SetStackTrace_ShouldGetCorrectValue()
        {
            var expect = "@#23123[." ;
            _log.exceptionStackTrace = expect;
            Assert.AreEqual(expect, _log.exceptionStackTrace);
        }

        [Test]
        public void LogMessageMobile_SetInnerExceptionType_ShouldGetCorrectValue()
        {
            var expect = ".";
            _log.innerExceptionType = expect;
            Assert.AreEqual(expect, _log.innerExceptionType);
        }

        [Test]
        public void LogMessageMobile_SetInnerExceptionMessage_ShouldGetCorrectValue()
        {
            var expect = "inner msg";
            _log.innerExceptionMessage = expect;
            Assert.AreEqual(expect, _log.innerExceptionMessage);
        }

        [Test]
        public void LogMessageMobile_SetInnerExceptionStackTrace_ShouldGetCorrectValue()
        {
            var expect = ".";
            _log.innerExceptionStackTrace = expect;
            Assert.AreEqual(expect, _log.innerExceptionStackTrace);
        }

        [Test]
        public void LogMessageMobile_SetApi_ShouldGetCorrectValue()
        {
            var expect = "downloadKeys";
            _log.api = expect;
            Assert.AreEqual(expect, _log.api);
        }

        [Test]
        public void LogMessageMobile_SetErrorCode_ShouldGetCorrectValue()
        {
            var expect = 204;
            _log.apiErrorCode = expect;
            Assert.AreEqual(expect, _log.apiErrorCode);
        }

        [Test]
        public void LogMessageMobile_SetApiErrorMessage_ShouldGetCorrectValue()
        {
            var expect = "213123";
            _log.apiErrorMessage = expect;
            Assert.AreEqual(expect, _log.apiErrorMessage);
        }

        [Test]
        public void LogMessageMobile_SetReportedTime_ShouldGetCorrectValue()
        {
            var expect = "12:00:00";
            _log.reportedTime = expect;
            Assert.AreEqual(expect, _log.reportedTime);
        }

        [Test]
        public void LogMessageMobile_SetAdditionalInfo_ShouldGetCorrectValue()
        {
            var expect = "abcsdfsfd";
            _log.additionalInfo = expect;
            Assert.AreEqual(expect, _log.additionalInfo);
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
