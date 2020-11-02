using NUnit.Framework;
using DIGNDB.App.SmitteStop.API.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExposureConfigurationServiceTests
    {
   
        [Test]
        public void ExposureConfigurationService_NotSetValueForConfiguration_ShouldDefaultObject()
        {
            var service = new ExposureConfigurationService();
            var config = service.GetConfiguration();
            Assert.AreEqual(0, config.DurationWeight);
            Assert.AreEqual(0, config.DaysSinceLastExposureWeight);
            Assert.AreEqual(0, config.TransmissionRiskWeight);
            Assert.AreEqual(0, config.MinimumRiskScore);
            Assert.AreEqual(0, config.AttenuationWeight);
            Assert.IsNull(config.AttenuationScores);
            Assert.IsNull(config.DurationAtAttenuationThresholds);
            Assert.IsNull(config.DaysSinceLastExposureScores);
            Assert.IsNull(config.DurationScores);
            Assert.IsNull(config.DurationAtAttenuationThresholds);

        }
    }
}
