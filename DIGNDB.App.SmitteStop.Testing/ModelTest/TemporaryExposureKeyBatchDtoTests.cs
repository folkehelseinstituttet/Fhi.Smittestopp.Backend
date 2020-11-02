using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Collections.Generic;
using System.Text;
using System;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class TemporaryExposureKeyBatchDtoTests
    {
        TemporaryExposureKeyBatchDto _dto;

        [SetUp]
        public void initBatch()
        {
            _dto = new TemporaryExposureKeyBatchDto();
        }

        [Test]
        public void TemporaryExposureKeyBatchDto_SetKeys_ShouldReturnCorrectValue()
        {
            var expect = new List<TemporaryExposureKeyDto>() {
                new TemporaryExposureKeyDto()
                {
                    key = new byte[TemporaryExposureKeyDto.KeyLength]  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    rollingDuration = "2.00:00:00",
                    rollingStart = DateTime.UtcNow,
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_HIGH
                },
                new TemporaryExposureKeyDto()
                {
                    key = new byte[TemporaryExposureKeyDto.KeyLength]  { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                    rollingDuration = "2.00:00:00",
                    rollingStart = DateTime.UtcNow,
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
                }
            };
            _dto.keys = expect;
            CollectionAssert.AreEqual(expect, _dto.keys);
        }

        [Test]
        public void TemporaryExposureKeyBatchDto_SetPayload_ShouldReturnCorrectValue()
        {
            var expect = "payload";
            _dto.deviceVerificationPayload = expect;
            Assert.AreEqual(expect, _dto.deviceVerificationPayload);
        }

        [Test]
        public void TemporaryExposureKeyBatchDto_SetPlatforms_ShouldReturnCorrectValue()
        {
            var expect = "ios";
            _dto.platform = expect;
            Assert.AreEqual(expect, _dto.platform);
        }

        [Test]
        public void TemporaryExposureKeyBatchDto_SetRegions_ShouldReturnCorrectValue()
        {
            var expect = new List<string>() { "DK", "abc", "VN" };
            _dto.regions = expect;
            CollectionAssert.AreEqual(expect, _dto.regions);
        }

        [Test]
        public void TemporaryExposureKeyBatchDto_SetAppPackageName_ShouldReturnCorrectValue()
        {
            var expect = "packageName";
            _dto.appPackageName = expect;
            Assert.AreEqual(expect, _dto.appPackageName);
        }
    }
}
