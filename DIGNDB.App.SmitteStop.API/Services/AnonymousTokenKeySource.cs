using System;
using System.Collections.Generic;
using System.Linq;
using DIGNDB.App.SmitteStop.API.Contracts;
using Org.BouncyCastle.Asn1.X9;
using System.Security.Cryptography.X509Certificates;
using DIGNDB.App.SmitteStop.API.Exceptions;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Math;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class AnonymousTokenKeySource : IAnonymousTokenKeySource
    {
        private readonly AnonymousTokenKeyStoreConfiguration _config;

        private RollingKeyPairGenerator _rollingKeyPairGenerator;

        private RollingKeyPairGenerator RollingKeyPairGenerator => _rollingKeyPairGenerator ??= CreateKeyPairGenerator();

        public AnonymousTokenKeySource(IOptions<AnonymousTokenKeyStoreConfiguration> config)
        {
            _config = config.Value;
        }

        public X9ECParameters ECParameters { get; } = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1);

        public BigInteger GetPrivateKey(string keyIdString)
        {
            if (!long.TryParse(keyIdString, out var keyId))
            {
                throw new FormatException($"Provided {nameof(keyIdString)} could not be parsed to a long number.");
            }

            if (!GetActiveKeyIds().Contains(keyId))
            {
                throw new ArgumentException($"Provided {nameof(keyIdString)} does not match one of the currently active key IDs.");
            }

            return RollingKeyPairGenerator.GetPrivateKey(keyId);
        }

        private IEnumerable<long> GetActiveKeyIds()
        {
            return new[]
            {
                // Past limit for rollover (potentially previous key)
                DateTimeOffset.UtcNow - _config.KeyRotationRollover,
                // Current time for current key
                DateTimeOffset.UtcNow
            }.Select(ToIntervalNumber).Distinct();
        }

        private long ToIntervalNumber(DateTimeOffset pointInTime)
        {
            return pointInTime.ToUnixTimeSeconds() / Convert.ToInt64(_config.KeyRotationInterval.TotalSeconds);
        }

        private RollingKeyPairGenerator CreateKeyPairGenerator()
        {
            var masterCertificate = LocateLocalCertificate(_config.CertificateThumbPrint);
            return new RollingKeyPairGenerator(masterCertificate, ECParameters);
        }

        private static X509Certificate2 LocateLocalCertificate(string thumbprint)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            var enumerator = certCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            throw new MissingCertificateException($"Certificate with thumbprint {thumbprint} not found");
        }
    }
}