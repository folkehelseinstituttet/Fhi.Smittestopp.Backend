using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Proto;
using NUnit.Framework;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using TemporaryExposureKey = DIGNDB.App.SmitteStop.Domain.Db.TemporaryExposureKey;

namespace DIGNDB.App.SmitteStop.Testing.UtilTest
{
    [TestFixture]
    public class ExposureBatchFileUtilTests
    {
        private readonly string _pemKeyFromGoogle = @"-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIE7yE32GaV/+qZ2tlOpdZRIXc9SJsyT5QhDJd9njZ+kcoAoGCCqGSM49
AwEHoUQDQgAEml59itec9qzwVojreLXdPNRsUWzfYHc1cKvIIi6/H56AJS/kZEYQ
nfDpxrgyGhdAm+pNN2GAJ3XdnQZ1Sk4amg==
-----END EC PRIVATE KEY-----";
        private readonly string _pemPublicKeyFromGoogle = @"-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEml59itec9qzwVojreLXdPNRsUWzf
YHc1cKvIIi6/H56AJS/kZEYQnfDpxrgyGhdAm+pNN2GAJ3XdnQZ1Sk4amg==
-----END PUBLIC KEY-----";
        private readonly string _pemFilePath = string.Format("{0}{1}", System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0], "\\test.pem");
        private TemporaryExposureKeyExport _exportBatch;


        [SetUp]
        public void Init()
        {
           CreatePemFile();
           SetupTemporaryExposureKeyExport();
        }

        private void CreatePemFile()
        {
            File.WriteAllText(_pemFilePath, _pemKeyFromGoogle);
        }

        private void SetupTemporaryExposureKeyExport()
        {
            var data = new List<TemporaryExposureKey> {
                new TemporaryExposureKey()
                {
                    CreatedOn = DateTime.UtcNow.Date,
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData1"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW,
                },
                new TemporaryExposureKey()
                {
                    CreatedOn = DateTime.UtcNow.Date.AddDays(-12),
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData2"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_HIGH,
                }
            };
            ExposureKeyMapper mapper = new ExposureKeyMapper();
            _exportBatch = mapper.FromEntityToProtoBatch(data);
        }

        [Test]
        public void CreateBinAsync_PassData_ShouldReturnStreamWithCorrectHeader()
        {
            var returnStream = ExposureBatchFileUtil.CreateBinAsync(_exportBatch);
            returnStream.Wait();
            var result = returnStream.Result;
            Assert.IsNotNull(result);

            StreamReader sr = new StreamReader(result);
            string content = sr.ReadToEnd();
            string HeaderContents = "EK Export v1    ";
            Assert.AreEqual(HeaderContents, content.Substring(0, 16));
        }

        [Test]
        public void CreateSignedFileAsync_HaveData_ShouldContainBinAndSigFiles()
        {
            var utils = new ExposureBatchFileUtil(_pemFilePath);
            var returnStream = utils.CreateSignedFileAsync(_exportBatch);
            returnStream.Wait();
            var result = returnStream.Result;
            Assert.IsNotNull(result);

            List<string> fileNameInZipStreams = new List<string>();
            List<string> expectFileNameInZip = new List<string>() { "export.bin", "export.sig" };
            using (var archive = new ZipArchive(result))
            {
                var entries = archive.Entries;
                foreach(ZipArchiveEntry entry in entries)
                {
                    fileNameInZipStreams.Add(entry.Name);
                }
            }
            CollectionAssert.AreEqual(expectFileNameInZip, fileNameInZipStreams);
        }

        [Test]
        public void SignData_WithValidKeys_ShouldReturnValidSignature()
        {
            byte[] sampleData = Encoding.UTF8.GetBytes("sample data");
            var signedData = ExposureBatchFileUtil.SignData(sampleData, _pemKeyFromGoogle);
            var valid = VerifySignedData(sampleData, signedData, _pemPublicKeyFromGoogle);
            Assert.AreEqual(valid, true);
        }

        private bool VerifySignedData(byte[] data, byte[] signedData, string publicKeyPem)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            var reader = new StringReader(publicKeyPem);
            PemReader pr = new PemReader(reader);
            ECPublicKeyParameters publicKey = (ECPublicKeyParameters)pr.ReadObject();
            signer.Init(false, publicKey);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.VerifySignature(signedData);
        }
    }
}
