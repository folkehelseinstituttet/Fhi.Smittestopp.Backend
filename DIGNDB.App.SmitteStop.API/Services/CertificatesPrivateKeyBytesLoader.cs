using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class CertificatePrivateKeyBytesLoader
    {
        private readonly PbeParameters _pbeParameters;

        public CertificatePrivateKeyBytesLoader()
        {
            _pbeParameters = new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc, HashAlgorithmName.SHA512, 1);
        }

        public byte[] ExtractPrivateKeyBytes(X509Certificate2 certificate)
        {
            using var ecdsaPrivateKey = certificate.GetECDsaPrivateKey();
            if (ecdsaPrivateKey != null)
            {
                return PlaintextExportParametersIncludingPrivateRegardlessOfExportPolicy(ecdsaPrivateKey).D;
            }

            var rsaPrivateKey = certificate.GetRSAPrivateKey();
            if (rsaPrivateKey != null)
            {
                var rsaParams = PlaintextExportParametersIncludingPrivateRegardlessOfExportPolicy(rsaPrivateKey);
                return rsaParams.D.Concat(rsaParams.P).Concat(rsaParams.Q).ToArray();
            }

            throw new CertificatePrivateKeyBytesLoaderException($"Unsupported private key for certificate {certificate.Thumbprint}");
        }

        private ECParameters PlaintextExportParametersIncludingPrivateRegardlessOfExportPolicy(ECDsa ecdsaKey)
        {
            if (ecdsaKey is ECDsaCng ecdsaCng && CngKeyNeedsPlaintextWorkaround(ecdsaCng.Key))
            {
                EnsureCanBeExported(ecdsaCng.Key);

                // Workaround if AllowExport is set, but missing AllowPlaintextExport
                var password = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                var exportedKey = ecdsaKey.ExportEncryptedPkcs8PrivateKey(password, _pbeParameters);
                using var tmpEcdsaKey = ECDsa.Create() ?? throw new CertificatePrivateKeyBytesLoaderException("Unable to create an instance of the default ECDsa implementation");
                tmpEcdsaKey.ImportEncryptedPkcs8PrivateKey(password, exportedKey, out _);
                return tmpEcdsaKey.ExportParameters(true);
            }

            return ecdsaKey.ExportParameters(true);
        }

        private RSAParameters PlaintextExportParametersIncludingPrivateRegardlessOfExportPolicy(RSA rsaKey)
        {
            if (rsaKey is RSACng rsaCng && CngKeyNeedsPlaintextWorkaround(rsaCng.Key))
            {
                EnsureCanBeExported(rsaCng.Key);

                // Workaround if AllowExport is set, but missing AllowPlaintextExport
                var password = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                var exportedKey = rsaKey.ExportEncryptedPkcs8PrivateKey(password, _pbeParameters);
                using var tmpRsaKey = RSA.Create() ?? throw new CertificatePrivateKeyBytesLoaderException("Unable to create an instance of the default RSA implementation");
                tmpRsaKey.ImportEncryptedPkcs8PrivateKey(password, exportedKey, out _);
                return tmpRsaKey.ExportParameters(true);
            }

            return rsaKey.ExportParameters(true);
        }

        private static bool CngKeyNeedsPlaintextWorkaround(CngKey cngKey)
        {
            return !cngKey.ExportPolicy.HasFlag(CngExportPolicies.AllowPlaintextExport);
        }

        private static void EnsureCanBeExported(CngKey cngKey)
        {
            if (!cngKey.ExportPolicy.HasFlag(CngExportPolicies.AllowExport))
            {
                throw new CertificatePrivateKeyBytesLoaderException("Certificate export policy does not allow export of private key");
            }
        }
    }

    public class CertificatePrivateKeyBytesLoaderException : Exception
    {
        public CertificatePrivateKeyBytesLoaderException(string message) : base(message)
        {

        }
    }
}
