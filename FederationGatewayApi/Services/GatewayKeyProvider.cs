using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace FederationGatewayApi.Services
{
    public class GatewayKeyProvider : IGatewayKeyProvider
    {
        private readonly IX509StoreWrapper _x509StoreWrapper;
        private readonly ISha256Wrapper _sha256Wrapper;
        private readonly IBitConverterWrapper _bitConverterWrapper;
        private readonly IPrivateKeyFactoryWrapper _privateKeyFactoryWrapper;
        private readonly IX509CertificateParserWrapper _x509CertificateParser;

        public GatewayKeyProvider(
            [NotNull] string authenticationCertificateFingerprint = null,
            [NotNull] string signingCertificateFingerprint = null,
            [NotNull] IX509StoreWrapper x509StoreWrapper = null,
            [NotNull] ISha256Wrapper sha256Wrapper = null,
            [NotNull] IBitConverterWrapper bitConverterWrapper = null,
            [NotNull] IPrivateKeyFactoryWrapper privateKeyFactoryWrapper = null,
            [NotNull] IX509CertificateParserWrapper x509CertificateParserWrapper = null
            )
        {
            _x509StoreWrapper = x509StoreWrapper ?? new IX509StoreWrapper.X509StoreWrapper();
            _sha256Wrapper = sha256Wrapper ?? new ISha256Wrapper.Sha256WrapperWrapper();
            _bitConverterWrapper = bitConverterWrapper ?? new IBitConverterWrapper.BitConverterWrapperWrapper();
            _privateKeyFactoryWrapper = privateKeyFactoryWrapper ?? new IPrivateKeyFactoryWrapper.PrivateKeyFactoryWrapperWrapper();
            _x509CertificateParser = x509CertificateParserWrapper ?? new IX509CertificateParserWrapper.X509CertificateParserWrapper();

            if (authenticationCertificateFingerprint != null && signingCertificateFingerprint != null)
                LoadCertificates(authenticationCertificateFingerprint, signingCertificateFingerprint);
        }

        public void LoadCertificates(string authenticationCertificateFingerprint, string signingCertificateFingerprint)
        {
            try
            {
                LoadCertificateFromCurrentUserStore(authenticationCertificateFingerprint, signingCertificateFingerprint);
            }
            catch (Exception exception) when (
                exception is ArgumentException ||     // Couldn't find certificate in CurrentUser store.

                exception is CryptographicException)  // Found certificate but couldn't load the private key
                                                      // either because it wasn't marked as exportable
                                                      // or the user didn't have permissions to read the private key
            {
                LoadCertificateFromLocalMachineStore(authenticationCertificateFingerprint, signingCertificateFingerprint);
            }
        }

        private void LoadCertificateFromLocalMachineStore(string authenticationCertificateFingerprint, string signingCertificateFingerprint)
        {
            LoadCertificatesFromLocation(authenticationCertificateFingerprint, signingCertificateFingerprint, StoreLocation.LocalMachine);
        }

        private void LoadCertificateFromCurrentUserStore(string authenticationCertificateFingerprint, string signingCertificateFingerprint)
        {
            LoadCertificatesFromLocation(authenticationCertificateFingerprint, signingCertificateFingerprint, StoreLocation.CurrentUser);
        }

        private void LoadCertificatesFromLocation(string authenticationCertificateFingerprint,
            string signingCertificateFingerprint, StoreLocation storeLocation)
        {
            _x509StoreWrapper.Initialize(StoreName.My, storeLocation);

            _x509StoreWrapper.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate2 certificate in _x509StoreWrapper.Certificates)
            {
                using var hasher = _sha256Wrapper.Create();

                var hash = _bitConverterWrapper.ToString(hasher.ComputeHash(certificate.RawData)).Replace("-", "");
                if (hash == authenticationCertificateFingerprint)
                {
                    AuthenticationPrivateKey = _privateKeyFactoryWrapper.CreateKey(certificate.PrivateKey.ExportPkcs8PrivateKey());
                    AuthenticationCertificate = certificate;
                }
                if (hash == signingCertificateFingerprint)
                {
                    SigningPrivateKey = _privateKeyFactoryWrapper.CreateKey(certificate.PrivateKey.ExportPkcs8PrivateKey());
                    SigningCertificate = _x509CertificateParser.ReadCertificate(certificate.GetRawCertData());
                }
            }

            AuthenticationCertificateFingerprint = authenticationCertificateFingerprint;
            //ValidateCertificates(authenticationCertificateFingerprint, signingCertificateFingerprint);
        }

        private void ValidateCertificates(string authenticationCertificateFingerprint, string signingCertificateFingerprint)
        {
            if (SigningCertificate == null)
            {
                throw new ArgumentException($"SigningCertificate with fingerprint {signingCertificateFingerprint} not found! Please add valid certificate to correct Windows Certificate Store.");
            }

            if (SigningPrivateKey == null)
            {
                throw new ArgumentException("SigningPrivateKey not Found!");
            }

            if (AuthenticationCertificate == null)
            {
                throw new ArgumentException($"AuthenticationCertificate with fingerprint {authenticationCertificateFingerprint} not found! Please add valid certificate to correct Windows Certificate Store.");
            }

            if (AuthenticationPrivateKey == null)
            {
                throw new ArgumentException("AuthenticationPrivateKey not Found!");
            }
        }

        public Org.BouncyCastle.X509.X509Certificate SigningCertificate { get; set; }

        public AsymmetricKeyParameter SigningPrivateKey { get; set; }

        public AsymmetricKeyParameter AuthenticationPrivateKey { get; set; }

        public X509Certificate2 AuthenticationCertificate { get; set; }

        public string AuthenticationCertificateFingerprint { get; set; }
    }

    public interface IX509StoreWrapper
    {
        void Initialize(StoreName my, StoreLocation localMachine);
        void Open(OpenFlags readOnly);
        X509Certificate2Collection Certificates { get; }

        public class X509StoreWrapper : IX509StoreWrapper
        {
            private X509Store _x509Store;

            public void Initialize(StoreName my, StoreLocation localMachine)
            {
                _x509Store = new X509Store(my, localMachine);
            }

            public void Open(OpenFlags readOnly)
            {
                _x509Store.Open(readOnly);
            }

            public X509Certificate2Collection Certificates => _x509Store.Certificates;
        }
    }

    public interface ISha256Wrapper
    {
        IHasherWrapper Create();

        public class Sha256WrapperWrapper : ISha256Wrapper
        {
            public IHasherWrapper Create()
            {
                return new IHasherWrapper.Sha256HasherWrapperWrapper(SHA256.Create());
            }
        }
    }

    public interface IHasherWrapper : IDisposable
    {
        byte[] ComputeHash(byte[] certificateRawData);

        public class Sha256HasherWrapperWrapper : IHasherWrapper
        {
            private readonly SHA256 _sha256;

            public Sha256HasherWrapperWrapper(SHA256 sha256)
            {
                _sha256 = sha256;
            }

            public void Dispose()
            {
                _sha256.Dispose();
            }

            public byte[] ComputeHash(byte[] certificateRawData)
            {
                return _sha256.ComputeHash(certificateRawData);
            }
        }
    }


    public interface IBitConverterWrapper
    {
        string ToString(byte[] value);

        public class BitConverterWrapperWrapper : IBitConverterWrapper
        {
            public string ToString(byte[] value)
            {
                return BitConverter.ToString(value);
            }
        }
    }

    public interface IPrivateKeyFactoryWrapper
    {
        AsymmetricKeyParameter CreateKey(byte[] exportPkcs8PrivateKey);

        public class PrivateKeyFactoryWrapperWrapper : IPrivateKeyFactoryWrapper
        {
            public AsymmetricKeyParameter CreateKey(byte[] exportPkcs8PrivateKey)
            {
                return PrivateKeyFactory.CreateKey(exportPkcs8PrivateKey);
            }
        }
    }

    public interface IX509CertificateParserWrapper
    {
        X509Certificate ReadCertificate(byte[] getRawCertData);

        public class X509CertificateParserWrapper : IX509CertificateParserWrapper
        {
            public X509Certificate ReadCertificate(byte[] getRawCertData)
            {
                return new X509CertificateParser().ReadCertificate(getRawCertData);
            }
        }
    }
}
