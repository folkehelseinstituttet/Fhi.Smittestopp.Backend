using DIGNDB.App.SmitteStop.Core.Models;
using NUnit.Framework;
using System;

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
        public void TemporaryExposureKeyDto_SetRollingDurationSpan_ShouldReturnCorrectValue()
        {
            var expect = "2.00:00:00.000";
            _dto.rollingDuration = expect;
            Assert.That(_dto.rollingDurationSpan, Is.EqualTo(TimeSpan.Parse(expect)));
        }

    }
}
