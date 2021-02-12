using DIGNDB.App.SmitteStop.Core.Helpers;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.UtilTest
{
    [TestFixture]
    public class ConfigEncryptionHelperTests
    {
        private byte[] s_aditionalEntropy = { 2, 1, 6, 8, 11 };
        
        [Test]
        [TestCase("testEncrypt")]
        [TestCase("abc123@[.]")]
        [TestCase("===@Nghjt")]
        public void ConfigEncryptionHelper_UnprotectedData_ShouldReturnString(string expectValue)
        {
            var encryptedExpectValue = ProtectedData.Protect(Encoding.UTF8.GetBytes(expectValue), 
                s_aditionalEntropy, DataProtectionScope.CurrentUser);
            var encryptedExpectValueToBase64 = Convert.ToBase64String(encryptedExpectValue);
            var decryptedValue = ConfigEncryptionHelper.UnprotectString(encryptedExpectValueToBase64);
            Assert.AreEqual(expectValue, decryptedValue);
        }

        [Test]
        [TestCase("testEncrypt")]
        [TestCase("abc123@[.]")]
        [TestCase("===@Nghjt")]
        public void ConfigEncryptionHelper_ProtectedData_ShouldReturnSameDataWhenDecrypted(string data)
        {
            var encryptedData = ConfigEncryptionHelper.ProtectString(data);
            
            //decrypteding to check
            var unprotectData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData),
                s_aditionalEntropy, DataProtectionScope.CurrentUser);
            var expectData = Encoding.UTF8.GetString(unprotectData);
            Assert.AreEqual(expectData, data);
        }
    }
}
