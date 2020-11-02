using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class ExposureConfigurationTests
    {
        ExposureConfiguration _config;

        [SetUp]
        public void initExposureConfiguration()
        {
            _config = new ExposureConfiguration();
        }

        [Test]
        public void ExposureConfiguration_SetMinimumRiskScore_ShouldGetSameValue()
        {
            var expectRiskScore = 1;
            _config.MinimumRiskScore = expectRiskScore;
            Assert.AreEqual(expectRiskScore, _config.MinimumRiskScore);
        }

        [Test]
        public void ExposureConfiguration_SetAttenuationScores_ShouldGetSameValue()
        {
            var expectAttenuationScore = new int[]{ 1, 2, 3, 4, 5, 6, 7, 8 };
            _config.AttenuationScores = expectAttenuationScore;
            Assert.AreEqual(expectAttenuationScore, _config.AttenuationScores);
        }

        [Test]
        public void ExposureConfiguration_SetAttenuationWeight_ShouldGetSameValue()
        {
            var expectAttenuationWeight = 1;
            _config.AttenuationWeight = expectAttenuationWeight;
            Assert.AreEqual(expectAttenuationWeight, _config.AttenuationWeight);
        }

        [Test]
        public void ExposureConfiguration_SetDaysSinceLastExposureScores_ShouldGetSameValue()
        {
            var expectDaysSinceLastExposureScores = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            _config.DaysSinceLastExposureScores = expectDaysSinceLastExposureScores;
            Assert.AreEqual(expectDaysSinceLastExposureScores, _config.DaysSinceLastExposureScores);
        }

        [Test]
        public void ExposureConfiguration_SetDaysSinceLastExposureWeight_ShouldGetSameValue()
        {
            var expectDaysSinceLastExposureWeight = 1;
            _config.DaysSinceLastExposureWeight = expectDaysSinceLastExposureWeight;
            Assert.AreEqual(expectDaysSinceLastExposureWeight, _config.DaysSinceLastExposureWeight);
        }

        [Test]
        public void ExposureConfiguration_SetDurationScores_ShouldGetSameValue()
        {
            var expectDurationScores = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            _config.DurationScores = expectDurationScores;
            Assert.AreEqual(expectDurationScores, _config.DurationScores);
        }

        [Test]
        public void ExposureConfiguration_SetDurationWeight_ShouldGetSameValue()
        {
            var expectDurationWeight = 1;
            _config.DurationWeight = expectDurationWeight;
            Assert.AreEqual(expectDurationWeight, _config.DurationWeight);
        }

        [Test]
        public void ExposureConfiguration_SetTransmissionRiskScores_ShouldGetSameValue()
        {
            var expectTransmissionRiskScores = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            _config.TransmissionRiskScores = expectTransmissionRiskScores;
            Assert.AreEqual(expectTransmissionRiskScores, _config.TransmissionRiskScores);
        }

        [Test]
        public void ExposureConfiguration_SetTransmissionRiskWeight_ShouldGetSameValue()
        {
            var expectTransmissionRiskWeight = 1;
            _config.TransmissionRiskWeight = expectTransmissionRiskWeight;
            Assert.AreEqual(expectTransmissionRiskWeight, _config.TransmissionRiskWeight);
        }

        [Test]
        public void ExposureConfiguration_SetDurationAtAttenuationThresholds_ShouldGetSameValue()
        {
            var expectDurationAtAttenuationThresholds = new int[] { 1, 250 };
            _config.DurationAtAttenuationThresholds = expectDurationAtAttenuationThresholds;
            Assert.AreEqual(expectDurationAtAttenuationThresholds, _config.DurationAtAttenuationThresholds);
        }
    }
}
