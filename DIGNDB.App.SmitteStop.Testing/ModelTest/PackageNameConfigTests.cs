using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class PackageNameConfigTests
    {
        [Test]
        public void PackageNameConfig_SetValueForApple_ShouldGetSameValue()
        {
            string expectedAppleName = "AppleName";
            PackageNameConfig config = new PackageNameConfig();
            config.Apple = expectedAppleName;
            Assert.AreEqual(expectedAppleName, config.Apple);
        }

        [Test]
        public void PackageNameConfig_SetValueForGoogle_ShouldGetSameValue()
        {
            string expectedGoogleName = "GoogleName";
            PackageNameConfig config = new PackageNameConfig();
            config.Google = expectedGoogleName;
            Assert.AreEqual(expectedGoogleName, config.Google);
        }
    }
}
