using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Configuration;

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
        public void KeyValidationConfiguration_SetRegions_ShouldGetSameValue()
        {
            List<string> expectedRegions = new List<string>() { "DK", "Test Region" };
            KeyValidationConfiguration config = new KeyValidationConfiguration();
            config.Regions = expectedRegions;
            CollectionAssert.AreEqual(expectedRegions, config.Regions);
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
