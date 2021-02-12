using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Domain.Dto;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.Json;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class LogMessageValidatorTests
    {
        IDictionary<string, string> _patternDictionary;

        [SetUp]
        public void init()
        {
            _patternDictionary = InitializePatternDictionary();
        }

        [Test]
        [TestCase("123")]
        [TestCase("absh1/!@.l")]
        [TestCase("10-04-2023")]
        public void ValidateLogMobileMessageDateTimeFormats_GiveInvalidDateTimeFormat_ShouldThrowError(string invalidDate)
        {
            LogMessageMobile log = new LogMessageMobile();
            log.reportedTime = invalidDate;
            var expectExceptionMsg = "Invalid datetime format";
            LogMessageValidator validator = new LogMessageValidator();
            var exception = Assert.Throws<JsonException>(() => validator.ValidateLogMobileMessageDateTimeFormats(log));
            Assert.That(exception.Message, Is.EqualTo(expectExceptionMsg));
        }

        [Test]
        [TestCaseSource(nameof(LogMessageWithInvalidPattern))]
        public void ValidateLogMobileMessagePatterns_LogMessageHasInValidData_ShouldThrowError(LogMessageMobile log)
        {
            var expectExceptionMsg = "Invalid pattern";
            LogMessageValidator validator = new LogMessageValidator();
            var exception = Assert.Throws<JsonException>(() => validator.ValidateLogMobileMessagePatterns(log, _patternDictionary));
            Assert.That(exception.Message, Is.EqualTo(expectExceptionMsg));
        }

        [Test]
        public void SanitizeAndShortenTextFields_GiveValueLargerThanMaxLimit_TheReturnValueShouldBeTrimmed()
        {
            LogMessageMobile log = new LogMessageMobile();
            var longMessage = "This is a longgggggggggg message";
            log.innerExceptionMessage = longMessage;
            log.deviceOSVersion = longMessage;
            var expectMaxLength = 15;

            LogMessageValidator validator = new LogMessageValidator();
            validator.SanitizeAndShortenTextFields(log, expectMaxLength);
            Assert.That(log.innerExceptionMessage.Length, Is.EqualTo(expectMaxLength));
            Assert.That(log.innerExceptionMessage, Is.EqualTo(longMessage.Substring(0, expectMaxLength)));

            Assert.That(log.deviceOSVersion.Length, Is.EqualTo(expectMaxLength));
            Assert.That(log.deviceOSVersion, Is.EqualTo(longMessage.Substring(0, expectMaxLength)));
        }

        [Test]
        public void SanitizeAndShortenField_GiveValueLargerThanMaxLimit_TheReturnValueShouldBeTrimmed()
        {
            var longMessage = "This is a longgggggggggg message";
            var expectMaxLength = 15;
            var outputMessage = "";

            LogMessageValidator validator = new LogMessageValidator();
            validator.SanitizeAndShortenField(longMessage, s => outputMessage = s, expectMaxLength);
            Assert.That(outputMessage.Length, Is.EqualTo(expectMaxLength));
            Assert.That(outputMessage, Is.EqualTo(longMessage.Substring(0, expectMaxLength)));
        }

        private IDictionary<string, string> InitializePatternDictionary()
        {
            var logMobilePatternsDictionary = new Dictionary<string, string>();
            logMobilePatternsDictionary.Add("severityRegex", "^(ERROR|INFO|WARNING)$");
            logMobilePatternsDictionary.Add("positiveNumbersRegex", "^[0-9]\\d*$");
            logMobilePatternsDictionary.Add("buildVersionRegex", "^[1-9]{1}[0-9]*([.][0-9]*){1,2}?$");
            logMobilePatternsDictionary.Add("operationSystemRegex", "^(IOS|Android-Google|Android-Huawei|Unknown)$");
            logMobilePatternsDictionary.Add("deviceOSVersionRegex", "^[1-9]{1}[0-9]{0,2}([.][0-9]{1,3}){1,2}?$");
            return logMobilePatternsDictionary;
        }

        public static IEnumerable<LogMessageMobile> LogMessageWithInvalidPattern
        {
            get
            {
                yield return new LogMessageMobile()
                {
                    severity = "ERROR",
                    apiVersion = 1,
                    buildVersion = "123.123",
                    buildNumber = "-1",
                    deviceType = "ios",
                    deviceOSVersion = "iOS14"
                };
                yield return new LogMessageMobile()
                {
                    severity = "abc",
                    apiVersion = 1,
                    buildVersion = "123",
                    buildNumber = "1.0.1",
                    deviceType = "ios",
                    deviceOSVersion = "iOS14"
                };
                yield return new LogMessageMobile()
                {
                    severity = "WARNING",
                    apiVersion = -1,
                    buildVersion = "123.123",
                    buildNumber = "1.0.1",
                    deviceType = "ios",
                    deviceOSVersion = "iOS14"
                };
                yield return new LogMessageMobile()
                {
                    severity = "INFO",
                    apiVersion = 1,
                    buildVersion = "123.123",
                    buildNumber = "1.0.1",
                    deviceType = "test",
                    deviceOSVersion = "iOS14",
                };
            }
        }
    }
}
