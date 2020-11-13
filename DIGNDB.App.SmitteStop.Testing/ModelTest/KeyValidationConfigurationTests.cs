using DIGNDB.App.SmitteStop.Domain.Configuration;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class KeyValidationConfigurationTests
    {
        [Test]
        public void KeyValidationConfiguration_SetOutDatedKeysOffeset_ShouldGetSameValue()
        {
            int expectOutdateKeysDayOffset = 1;
            KeyValidationConfiguration config = new KeyValidationConfiguration();
            config.OutdatedKeysDayOffset = expectOutdateKeysDayOffset;
            Assert.AreEqual(expectOutdateKeysDayOffset, config.OutdatedKeysDayOffset);
        }

        [Test]
        public void KeyValidationConfiguration_SetPackageNames_ShouldGetSameValue()
        {
            PackageNameConfig packageNames = new PackageNameConfig()
            {
                Apple = "AppleName",
                Google = "GoogleName"
            };
            KeyValidationConfiguration config = new KeyValidationConfiguration();
            config.PackageNames = packageNames;
            Assert.AreEqual(packageNames.Apple, config.PackageNames.Apple);
            Assert.AreEqual(packageNames.Google, config.PackageNames.Google);
        }
    }
}
