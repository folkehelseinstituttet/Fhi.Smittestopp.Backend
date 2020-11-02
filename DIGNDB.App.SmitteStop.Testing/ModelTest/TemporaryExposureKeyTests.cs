using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Text;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class TemporaryExposureKeyTests
    {
        TemporaryExposureKey _entity;

        [SetUp]
        public void initEntity()
        {
            _entity = new TemporaryExposureKey();
        }

        [Test]
        public void TemporaryExposureKey_SetId_ShouldReturnCorrectValue()
        {
            var expect = Guid.NewGuid();
            _entity.Id = expect;
            Assert.AreEqual(expect, _entity.Id);
        }

        [Test]
        public void TemporaryExposureKey_SetKeyData_ShouldReturnCorrectValue()
        {
            var expect = Encoding.UTF8.GetBytes("keyData");
            _entity.KeyData = expect;
            Assert.AreEqual(expect, _entity.KeyData);
        }

        [Test]
        public void TemporaryExposureKey_SetRollingStart_ShouldReturnCorrectValue()
        {
            var expect = 123;
            _entity.RollingStartNumber = expect;
            Assert.AreEqual(expect, _entity.RollingStartNumber);
        }

        [Test]
        public void TemporaryExposureKey_SetRollingPeriod_ShouldReturnCorrectValue()
        {
            var expect = 144;
            _entity.RollingPeriod = expect;
            Assert.AreEqual(expect, _entity.RollingPeriod);
        }

        [Test]
        public void TemporaryExposureKey_SetTransmissionRiskLevel_ShouldReturnCorrectValue()
        {
            var expect = RiskLevel.RISK_LEVEL_LOW_MEDIUM;
            _entity.TransmissionRiskLevel = expect;
            Assert.AreEqual(expect, _entity.TransmissionRiskLevel);
        }

        [Test]
        public void TemporaryExposureKey_SetCreateOn_ShouldReturnCorrectValue()
        {
            var expect = DateTime.UtcNow;
            _entity.CreatedOn = expect;
            Assert.AreEqual(expect, _entity.CreatedOn);
        }
    }
}
