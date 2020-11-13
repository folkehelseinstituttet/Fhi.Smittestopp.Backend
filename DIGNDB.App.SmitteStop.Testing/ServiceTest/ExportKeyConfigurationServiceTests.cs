using DIGNDB.App.SmitteStop.API.Services;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExportKeyConfigurationServiceTests
    {
        [Test]
        public void GetConfiguration_HaveNoData_ShouldReturnDefaultValue()
        {
            ExportKeyConfigurationService service = new ExportKeyConfigurationService();
            var configuration = service.GetConfiguration();
            Assert.AreEqual(TimeSpan.Parse("00:00:00"), configuration.CurrentDayFileCaching);
            Assert.AreEqual(TimeSpan.Parse("00:00:00"), configuration.PreviousDayFileCaching);
            Assert.AreEqual(0, configuration.MaxKeysPerFile);
        }
    }
}
