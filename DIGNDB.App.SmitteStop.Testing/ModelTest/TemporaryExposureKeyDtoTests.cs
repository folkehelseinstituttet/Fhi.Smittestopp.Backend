using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Text;
using System;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class TemporaryExposureKeyDtoTests
    {
        TemporaryExposureKeyDto _dto;

        [SetUp]
        public void initModel()
        {
            _dto = new TemporaryExposureKeyDto();
        }

        [Test]
        public void TemporaryExposureKeyDto_SetKey_ShouldReturnCorrectValue()
        {
            var expect = Encoding.UTF8.GetBytes("keyData");
            _dto.key = expect;
            Assert.That(_dto.key, Is.EqualTo(expect));
        }

        [Test]
        public void TemporaryExposureKeyDto_SetRollingStart_ShouldReturnCorrectValue()
        {
            var expect = DateTime.UtcNow;
            _dto.rollingStart = expect;
            Assert.That(_dto.rollingStart, Is.EqualTo(expect));
        }

        [Test]
        public void TemporaryExposureKeyDto_SetRollingDurationSpan_ShouldReturnCorrectValue()
        {
            var expect = "2.00:00:00.000";
            _dto.rollingDuration = expect;
            Assert.That(_dto.rollingDurationSpan, Is.EqualTo(TimeSpan.Parse(expect)));
        }

        [Test]
        public void TemporaryExposureKeyDto_SetTransmissionRiskLevel_ShouldReturnCorrectValue()
        {
            var expect = RiskLevel.RISK_LEVEL_LOW_MEDIUM;
            _dto.transmissionRiskLevel = expect;
            Assert.That(_dto.transmissionRiskLevel, Is.EqualTo(expect));
        }
    }
}
