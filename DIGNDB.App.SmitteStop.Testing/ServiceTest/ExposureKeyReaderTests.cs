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
using System.IO;


namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExposureKeyReaderTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private ExposureKeyValidator _exposureKeyValidator;
        private ExposureKeyReader _exposureKeyReader;
        private TemporaryExposureKeyBatchDto _parameterArgument;
        private KeyValidationConfiguration _keyValidationConfiguration;

        private Mock<ILogger<ExposureKeyValidator>> _loggerValidator;
        private Mock<ILogger<ExposureKeyReader>> _loggerReader;

        [SetUp]
        public void Init()
        {
            SetupMockServices();

            _keyValidationConfiguration = new KeyValidationConfiguration()
            {
                OutdatedKeysDayOffset = 14,
                PackageNames = new PackageNameConfig() { Apple = "com.netcompany.smittestop-exposure-notification", Google = "com.netcompany.smittestop-exposure-notification" },
            };

            _exposureKeyValidator = new ExposureKeyValidator(_countryRepositoryMock.Object, _loggerValidator.Object);
            _exposureKeyReader = new ExposureKeyReader(_exposureKeyValidator, _keyValidationConfiguration, _loggerReader.Object);
            
            _parameterArgument = new TemporaryExposureKeyBatchDto()
            {
                appPackageName = "com.netcompany.smittestop-exposure-notification",
                platform = "Android",

                keys = new List<TemporaryExposureKeyDto>()
                {
                    new TemporaryExposureKeyDto()
                    {
                        key = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
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
                },
                isSharingAllowed = true
            };
        }

        private void SetupMockServices()
        {
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("AT")).Returns(new Country() { Code = "AT", VisitedCountriesEnabled = true });
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("FR")).Returns(new Country() { Code = "FR", VisitedCountriesEnabled = false });
            _countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("PL")).Returns((Country)null);
            _countryRepositoryMock.Setup(countryRepo => countryRepo.GetApiOriginCountry()).Returns(new Country() { Code = "DK", VisitedCountriesEnabled = false });
            _loggerValidator = new Mock<ILogger<ExposureKeyValidator>>();
            _loggerReader = new Mock<ILogger<ExposureKeyReader>>();
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldPassValidation()
        {
            _parameterArgument.isSharingAllowed = false;
            var paramsAsJson = _parameterArgument.ToJson();
            using var stream = GenerateStreamFromString(paramsAsJson);
            var parameters = _exposureKeyReader.ReadParametersFromBody(stream);

            Assert.That(parameters.Result.isSharingAllowed, Is.False, "SharingConsentGiven should be false");
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
