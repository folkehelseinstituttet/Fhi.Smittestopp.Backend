using DIGNDB.App.SmitteStop.Domain;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class ExportKeyConfigurationTests
    {
        [Test]
        public void PreviousDayFileCaching_SetValue_ShouldReturnCorrectValue()
        {
            TimeSpan expectPreviousDayCaching = TimeSpan.Parse("12.00:00:00.000");
            ExportKeyConfiguration config = new ExportKeyConfiguration();
            config.PreviousDayFileCaching = expectPreviousDayCaching;
            Assert.AreEqual(expectPreviousDayCaching, config.PreviousDayFileCaching);
        }

        [Test]
        public void CurrentDayFileCaching_SetValue_ShouldReturnCorrectValue()
        {
            TimeSpan expectCurrentDayCaching = TimeSpan.Parse("02:00:00.000");
            ExportKeyConfiguration config = new ExportKeyConfiguration();
            config.CurrentDayFileCaching = expectCurrentDayCaching;
            Assert.AreEqual(expectCurrentDayCaching, config.CurrentDayFileCaching);
        }

        [Test]
        public void MaxKeysPerFile_SetValue_ShouldReturnCorrectValue()
        {
            int expectedKeysPerFile = 1;
            ExportKeyConfiguration config = new ExportKeyConfiguration();
            config.MaxKeysPerFile = expectedKeysPerFile;
            Assert.AreEqual(expectedKeysPerFile, config.MaxKeysPerFile);
        }
    }
}
