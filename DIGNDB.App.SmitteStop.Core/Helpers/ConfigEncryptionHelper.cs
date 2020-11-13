using System;
using System.Security.Cryptography;
using System.Text;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class ConfigEncryptionHelper
    {
        static byte[] s_aditionalEntropy = { 2, 1, 6, 8, 11 };
        static Encoding _encoding = Encoding.UTF8;

        /// <param name="data"></param>
        /// <exception cref="CryptographicException">The decryption failed</exception>
        /// <exception cref="NotSupportedException">The operating system does not support this method.</exception>
        /// <exception cref="OutOfMemoryException">Out of memory</exception>
        /// <exception cref="System.PlatformNotSupportedException">.NET Core only: Calls to the Unprotect method are supported on Windows operating systems only.</exception>
        /// <exception cref="ArgumentException">data param cannot be null</exception>
        public static string ProtectString(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("data parameter cannot be null or empty");
            }
            byte[] secret = _encoding.GetBytes(data);
            var protectedBytes = ProtectedData.Protect(secret, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        /// <param name="data"></param>
        /// <exception cref="CryptographicException">The decryption failed</exception>
        /// <exception cref="NotSupportedException">The operating system does not support this method.</exception>
        /// <exception cref="OutOfMemoryException">Out of memory</exception>
        /// <exception cref="System.PlatformNotSupportedException">.NET Core only: Calls to the Unprotect method are supported on Windows operating systems only.</exception>
        /// <exception cref="ArgumentException">data param cannot be null</exception>
        public static string UnprotectString(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("data parameter cannot be null or empty");
            }

            var bytes = System.Convert.FromBase64String(data);
            var unprotectedBytes = ProtectedData.Unprotect(bytes, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            return _encoding.GetString(unprotectedBytes);
        }
    }
}
