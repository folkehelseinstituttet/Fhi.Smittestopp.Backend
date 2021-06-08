using FederationGatewayApi.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    [TestFixture]
    public class GatewayKeyProviderTests
    {
        private readonly Mock<IX509StoreWrapper> _x509StoreWrapperMock = new Mock<IX509StoreWrapper>();
        private readonly Mock<ISha256Wrapper> _sha256WrapperMock = new Mock<ISha256Wrapper>();
        private readonly Mock<IBitConverterWrapper> _bitConverterWrapperMock = new Mock<IBitConverterWrapper>();
        private readonly Mock<IPrivateKeyFactoryWrapper> _privateKeyFactoryWrapperMock = new Mock<IPrivateKeyFactoryWrapper>();
        private readonly Mock<IX509CertificateParserWrapper> _x509CertificateParserWrapperMock = new Mock<IX509CertificateParserWrapper>();

        private const string AuthenticationCertificateFingerprint = "A3C3E533CC9FEACA026F99F688F4488B5FC16BD0E6A80E6E0FC03760983DBF3F";
        private const string SigningCertificateFingerprint = "979673B55DB0B7E2B35B12CF2A342655F059314BC46323C43BCD3BFC82374BFB";

        [Test]
        public void Should_Read_Certificates_From_UserStore_First_Then_Local_Machine()
        {
            int callOrder = 0;

            _x509StoreWrapperMock.Setup(mock => mock.Initialize(StoreName.My, StoreLocation.CurrentUser))
                .Callback(() =>
                {
                    callOrder++;
                    callOrder.Should().Be(1);
                });
            _x509StoreWrapperMock.Setup(mock => mock.Initialize(StoreName.My, StoreLocation.LocalMachine))
                .Callback(() =>
                {
                    callOrder++;
                    callOrder.Should().Be(2);
                });

            try
            {
                var gatewayKeyProvider = new GatewayKeyProvider(
                    AuthenticationCertificateFingerprint,
                    SigningCertificateFingerprint,
                    _x509StoreWrapperMock.Object,
                    _sha256WrapperMock.Object,
                    _bitConverterWrapperMock.Object,
                    _privateKeyFactoryWrapperMock.Object,
                    _x509CertificateParserWrapperMock.Object);
            }
            catch (AssertionException)
            {
                throw;
            }
            catch (CryptographicException)
            {
                throw;
            }
            catch (Exception)
            {
                // ignored because testing only interaction with X509Store
            }
        }
    }
}