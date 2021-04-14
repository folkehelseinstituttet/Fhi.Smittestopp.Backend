using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExposureKeyValidatorTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private ExposureKeyValidator _exposureKeyValidator;
        private TemporaryExposureKeyBatchDto _parameterArgument;
        private KeyValidationConfiguration _configurationArgument;

        private Mock<ILogger<ExposureKeyValidator>> _logger;
        
        [SetUp]
        public void Init()
        {
            SetupMockServices();
            _exposureKeyValidator = new ExposureKeyValidator(_countryRepositoryMock.Object, _logger.Object);
            _parameterArgument = new TemporaryExposureKeyBatchDto()
                {
                    appPackageName = "com.netcompany.smittestop-exposure-notification",
                    platform = "Android",

                    keys = new List<TemporaryExposureKeyDto>()
                    {
                        new TemporaryExposureKeyDto()
                        {
                            key = new byte[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            rollingDuration = "1.00:00:00",
                            rollingStart = DateTime.UtcNow.Date.AddDays(-1),
                            transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                        }
                    },
                    regions = new List<string>
                    {
                        "dk"
                    },
                    visitedCountries = new List<string>
                    {
                        "AT"
                    }
                };

            _configurationArgument = new KeyValidationConfiguration()
            {
                OutdatedKeysDayOffset = 14,
                PackageNames = new PackageNameConfig() { Apple = "com.netcompany.smittestop-exposure-notification", Google = "com.netcompany.smittestop-exposure-notification" },
            };
        }

        private void SetupMockServices()
        {
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("AT")).Returns(new Country() { Code = "AT", VisitedCountriesEnabled = true });
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("FR")).Returns(new Country() { Code = "FR", VisitedCountriesEnabled = false });
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("PL")).Returns((Country)null);
            _countryRepositoryMock.Setup(countryRepo => countryRepo.GetApiOriginCountry()).Returns(new Country() { Code = "DK", VisitedCountriesEnabled = false });
            _logger = new Mock<ILogger<ExposureKeyValidator>>();
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldPassValidation()
        {
            Assert.DoesNotThrow(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument));
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectKeyCount()
        {
            _configurationArgument.OutdatedKeysDayOffset = 0;
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Incorrect key count.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowDuplicateKeyValues()
        {
            _parameterArgument.keys.Add(
                new TemporaryExposureKeyDto()
                {
                    key = _parameterArgument.keys.First().key,
                    rollingDuration = "1.00:00:00",
                    rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                });
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Duplicate key values.");
        }

        [TestCase(-16)]
        [TestCase(2)]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectStartDate(int value)
        {
            _parameterArgument.keys[0].rollingStart = _parameterArgument.keys[0].rollingStart.AddDays(value);
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Incorrect start date.");
        }

        [TestCase("1.00:00:01")]
        [TestCase("0.00:09:59")]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectSpan(string value)
        {
            _parameterArgument.keys.Add(
                new TemporaryExposureKeyDto()
                {
                    key = new byte[TemporaryExposureKeyDto.KeyLength] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    rollingDuration = value,
                    rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                });
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Incorrect span.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowNotFoundVisitedCountry()
        {
            _parameterArgument.visitedCountries = new List<string> { "PL" };

            Assert.Throws<ArgumentException>(
                () => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument,
                    _configurationArgument), $"ISO codes of countries not found in the database: {string.Join(", ", _parameterArgument.visitedCountries)}. ");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowDisabledCountry()
        {
            _parameterArgument.visitedCountries = new List<string> { "FR" };

            Assert.Throws<ArgumentException>(
                () => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument,
                    _configurationArgument), $"ISO codes of countries marked as disabled in the database: {string.Join(", ", _parameterArgument.visitedCountries)}. ");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectRegion()
        {
            _parameterArgument.regions = new List<string>() { "aaa" };
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Incorrect region.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectPackageName()
        {
            _parameterArgument.appPackageName = "test";
            Assert.Throws<ArgumentException>(() => _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument), "Incorrect package name.");
        }

        [TestCase(new byte[TemporaryExposureKeyDto.KeyLength - 1] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(new byte[TemporaryExposureKeyDto.KeyLength + 1] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_KeySize_ShouldReturnFalse(byte[] value)
        {
            var tempKey = new TemporaryExposureKeyDto()
            {
                key = value,
                rollingDuration = "1.00:00:00",
                rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
            };
            _parameterArgument.keys.Add(tempKey);
            _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument);
            Assert.IsFalse(_parameterArgument.keys.Contains(tempKey));
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_CorrectKey_ShouldReturnTrue()
        {
            var tempKeyWithCorrectLength = new TemporaryExposureKeyDto()
            {
                key = new byte[TemporaryExposureKeyDto.KeyLength] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
                rollingDuration = "1.00:00:00",
                rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
            };
            _parameterArgument.keys.Add(tempKeyWithCorrectLength);
            _exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(_parameterArgument, _configurationArgument);
            Assert.IsTrue(_parameterArgument.keys.Contains(tempKeyWithCorrectLength));
        }
    }
}
