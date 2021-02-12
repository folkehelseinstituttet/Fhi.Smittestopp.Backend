using DIGNDB.App.SmitteStop.Domain.Dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class AttestationTests
    {
        [Test]
        public void AttestationStatement_CallConstructorToSetValueForProperties_ShouldGetCorrectValueBack()
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();
            var expectNonce = Encoding.UTF8.GetBytes("nonce");
            var expectTimeStamp = 123;
            var expectApkPackageName = "PckName";
            var expectApkDigestSha256 = Encoding.UTF8.GetBytes("digest");
            var expectApkCertificateDigestSha256 = Encoding.UTF8.GetBytes("certificate");
            var expectCtsProfileMatch = false;
            var expectBasicIntegrity = true;

            claims.Add("nonce", Convert.ToBase64String(expectNonce));
            claims.Add("timestampMs", expectTimeStamp.ToString());
            claims.Add("apkPackageName", expectApkPackageName);
            claims.Add("apkDigestSha256", Convert.ToBase64String(expectApkDigestSha256));
            claims.Add("apkCertificateDigestSha256", Convert.ToBase64String(expectApkCertificateDigestSha256));
            claims.Add("ctsProfileMatch", expectCtsProfileMatch.ToString());
            claims.Add("basicIntegrity", expectBasicIntegrity.ToString());

            AttestationStatement attestation = new AttestationStatement(claims);

            Assert.AreEqual(expectNonce, attestation.Nonce);
            Assert.AreEqual(expectTimeStamp, attestation.TimestampMs);
            Assert.AreEqual(expectApkPackageName, attestation.ApkPackageName);
            Assert.AreEqual(expectApkDigestSha256, attestation.ApkDigestSha256);
            Assert.AreEqual(expectApkCertificateDigestSha256, attestation.ApkCertificateDigestSha256);
            Assert.AreEqual(expectCtsProfileMatch, attestation.CtsProfileMatch);
            Assert.AreEqual(expectBasicIntegrity, attestation.BasicIntegrity);
            CollectionAssert.AreEqual(claims, attestation.Claims);
        }
    }
}
