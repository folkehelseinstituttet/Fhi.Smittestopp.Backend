using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Helpers;
using System;
using FluentAssertions;

namespace DIGNDB.App.SmitteStop.Testing.UtilTest
{
    [TestFixture]
    public class ConfigEncryptionHelperTests
    {
        [TestCase("testEncrypt")]
        [TestCase("abc123@[.]")]
        [TestCase("===@Nghjt")]
        public void ConfigEncryptionHelper_UnprotectedData_ShouldReturnString(string expectValue)
        {
            Action unprotectString = () => ConfigEncryptionHelper.UnprotectString(null);
            unprotectString.Should().Throw<NotImplementedException>();
        }

        [TestCase("testEncrypt")]
        [TestCase("abc123@[.]")]
        [TestCase("===@Nghjt")]
        public void ConfigEncryptionHelper_ProtectedData_ShouldReturnSameDataWhenDecrypted(string data)
        {
            Action protectString = () => ConfigEncryptionHelper.ProtectString(data);
            protectString.Should().Throw<NotImplementedException>();
        }
    }
}
