using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.API.Services;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Math;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DIGNDB.App.SmitteStop.API
{
    public class RollingKeyPairGeneratorWithLocalCertificate : RollingKeyPairGenerator, IAnonymousTokenKeySource
    {
        public RollingKeyPairGeneratorWithLocalCertificate(string thumbprint)
            : base(LocateLocalCertificate(thumbprint), CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1))
        {
        }

        private static X509Certificate2 LocateLocalCertificate(string thumbprint)
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                var enumerator = certCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
                throw new Exception($"Certificate with thumbprint {thumbprint} not found");
            }
        }
    }
}