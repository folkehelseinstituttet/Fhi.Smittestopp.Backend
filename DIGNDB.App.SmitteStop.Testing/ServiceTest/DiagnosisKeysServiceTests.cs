using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using DIGNDB.App.SmitteStop.API.Services;
using Microsoft.Extensions.Configuration;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using System.IO;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Core.Services;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class DiagnosisKeysServiceTests
    {
        private Mock<IConfiguration> _configuration;
        private ExposureKeyMapper _mapper;
        private ExportKeyConfigurationService _exportKeyConfigurationService;
        private Mock<ITemporaryExposureKeyRepository> _temporaryExposureKeyRepository;

        private readonly string _pemKeyFromGoogle = @"-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIE7yE32GaV/+qZ2tlOpdZRIXc9SJsyT5QhDJd9njZ+kcoAoGCCqGSM49
AwEHoUQDQgAEml59itec9qzwVojreLXdPNRsUWzfYHc1cKvIIi6/H56AJS/kZEYQ
nfDpxrgyGhdAm+pNN2GAJ3XdnQZ1Sk4amg==
-----END EC PRIVATE KEY-----";

        private readonly string _pemFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0], "\\test.pem");

        [SetUp]
        public void Init()
        {
            CreatePemFile();
            _mapper = new ExposureKeyMapper();
            _exportKeyConfigurationService = new ExportKeyConfigurationService();
            _temporaryExposureKeyRepository = new Mock<ITemporaryExposureKeyRepository>();
            SetupMockConfiguration();

        }

        private void CreatePemFile()
        {
            File.WriteAllText(_pemFilePath, _pemKeyFromGoogle);
        }

        private void SetupMockConfiguration()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(config => config["KeyValidationRules:PackageNames:android"]).Returns("com.netcompany.smittestop_exposure_notification");
            _configuration.Setup(config => config["KeyValidationRules:PackageNames:ios"]).Returns("com.netcompany.smittestop-exposure-notification");
            _configuration.Setup(config => config["AppSettings:certificateThumbprint"]).Returns(_pemFilePath);
        }

        private IList<TemporaryExposureKey> CreateMockedListExposureKeys(DateTime expectDate)
        {
            var data = new List<TemporaryExposureKey> {
                new TemporaryExposureKey()
                {
                    CreatedOn = expectDate.Date,
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData1"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW,
                },
                new TemporaryExposureKey()
                {
                    CreatedOn = expectDate.Date.AddDays(-12),
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData2"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_HIGH,
                }
            };
            return data;
        }

        [Test]
        public void ExportDiagnosisKeys_HaveKeys_ShouldReturnStream()
        {
            DatabaseKeysToBinaryStreamMapperService toBinaryStreamMapperService = new DatabaseKeysToBinaryStreamMapperService(_mapper, _configuration.Object);
            var expectDate = DateTime.UtcNow;
            var data = CreateMockedListExposureKeys(expectDate);
            var streamResult = toBinaryStreamMapperService.ExportDiagnosisKeys(data);
            Assert.AreNotEqual(0,streamResult.Length);
        }

        [Test]
        public void ExportDiagnosisKeys_HaveNoKey_ShouldThrowException()
        {
            DatabaseKeysToBinaryStreamMapperService toBinaryStreamMapperService = new DatabaseKeysToBinaryStreamMapperService(_mapper, _configuration.Object);
            var data = new List<TemporaryExposureKey> {};
            var exception = Assert.Throws<InvalidOperationException>(() => toBinaryStreamMapperService.ExportDiagnosisKeys(data));
            Assert.AreEqual(exception.Message, "Sequence contains no elements");
        }
    }
}
