using Org.BouncyCastle.Crypto;
using System.Security.Cryptography.X509Certificates;

namespace FederationGatewayApi.Services
{
    public interface IGatewayKeyProvider
    {
        public Org.BouncyCastle.X509.X509Certificate SigningCertificate { get; }

        public AsymmetricKeyParameter SigningPrivateKey { get; }

        public AsymmetricKeyParameter AuthenticationPrivateKey  { get; }

        public X509Certificate2 AuthenticationCertificate { get; }

        string AuthenticationCertificateFingerprint { get; }

        void LoadCertificates(string authenticationCertificateFingerprint, string signingCertificateFingerprint);
    }
}