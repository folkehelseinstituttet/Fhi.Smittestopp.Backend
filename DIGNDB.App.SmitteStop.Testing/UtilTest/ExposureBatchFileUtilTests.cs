using DIGNDB.App.SmitteStop.Core.Contracts;
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
using System.Linq;
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
        private IEpochConverter _epochConverter;


        [SetUp]
        public void Init()
        {
            _epochConverter = new EpochConverter();

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
                new TemporaryExposureKey
                {
                    CreatedOn = DateTime.UtcNow.Date,
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData1"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW,
                    ReportType = ReportType.CONFIRMED_TEST,
                    DaysSinceOnsetOfSymptoms = 1
                },
                new TemporaryExposureKey
                {
                    CreatedOn = DateTime.UtcNow.Date.AddDays(-12),
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData2"),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_HIGH,
                    ReportType = ReportType.CONFIRMED_TEST,
                    DaysSinceOnsetOfSymptoms = 2
                }
            };
            ExposureKeyMapper mapper = new ExposureKeyMapper(_epochConverter);
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
                foreach (ZipArchiveEntry entry in entries)
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

        [Test]
        public void CreateSignedFileAsync_HaveData_AppDeserializationEqualsOriginalKeys()
        {
            // Arrange
            var utils = new ExposureBatchFileUtil(_pemFilePath);

            // Act
            var returnStream = utils.CreateSignedFileAsync(_exportBatch);
            returnStream.Wait();
            var result = returnStream.Result;
            
            // Assert
            Assert.IsNotNull(result);

            // Arrange
            using var archive = new ZipArchive(result);

            // Act
            var tempExposureKeyExport = ZipToTemporaryExposureKeyExport(archive);

            // Assert
            var keys = tempExposureKeyExport.Keys;
            using var enumerator = keys.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var batchKey = _exportBatch.Keys[i];
                Assert.AreEqual(current.RollingStartIntervalNumber, batchKey.RollingStartIntervalNumber);
                Assert.AreEqual(current.RollingPeriod, batchKey.RollingPeriod);
                Assert.AreEqual(current.KeyData, batchKey.KeyData);
                Assert.AreEqual(current.DaysSinceOnsetOfSymptoms, batchKey.DaysSinceOnsetOfSymptoms);
                Assert.AreEqual(current.ReportType, batchKey.ReportType);
                i++;
            }
        }

        public static TemporaryExposureKeyExport ZipToTemporaryExposureKeyExport(ZipArchive zipArchive)
        {
            ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry("export.bin");
            Stream stream = zipArchiveEntry.Open();
            byte[] bytes = ReadToEnd(stream);
            IEnumerable<byte> bytesSliced = bytes.Skip(16);
            return TemporaryExposureKeyExport.Parser.ParseFrom(bytesSliced.ToArray());
        }

        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
