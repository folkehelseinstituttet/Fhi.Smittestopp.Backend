namespace FederationGatewayApi.Services
{
    using DIGNDB.App.SmitteStop.Core.Contracts;
    using DIGNDB.App.SmitteStop.Domain;
    using FederationGatewayApi.Models.Proto;
    using Google.Protobuf;
    using Org.BouncyCastle.Cms;
    using Org.BouncyCastle.X509.Store;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class SignatureService : ISignatureService
    {
        private readonly IGatewayKeyProvider _gatewayKeyProvider;
        private readonly IEncodingService _encodingService;

        public SignatureService(IGatewayKeyProvider gatewayKeyProvider, IEncodingService encodingService)
        {
            _gatewayKeyProvider = gatewayKeyProvider;
            _encodingService = encodingService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="protoBatch"> The diagnosis key batch, from which the information to generate the bytes to verify are obtained. Keys inside the batch MUST be sorted</param>
        /// <returns></returns>
        public byte[] Sign(TemporaryExposureKeyGatewayBatchDto protoBatch, SortOrder keysSortOrder)
        {
            MemoryStream memStream = new MemoryStream();

            Func<byte[], string> keyEncodingForSortFunc = keySigBytes => _encodingService.EncodeToBase64(keySigBytes);
            var comparer = StringComparer.Ordinal;

            var query = protoBatch.Keys.Select(GenerateSignaturePayloadFromKey);
            query = (keysSortOrder == SortOrder.ASC) ? query.OrderBy(keyEncodingForSortFunc, comparer) : query.OrderByDescending(keyEncodingForSortFunc, comparer);

            var sortedKeySignatures = query.ToList();

            sortedKeySignatures.ForEach(sig =>
            {
                memStream.Write(sig);
            });

            return SignWithCertificate(memStream.ToArray());
        }

        private byte[] SignWithCertificate(byte[] bytesToSign)
        {
            var generator = new CmsSignedDataGenerator();

            var privKey = _gatewayKeyProvider.SigningPrivateKey;
            var certificate = _gatewayKeyProvider.SigningCertificate;

            var certificates = new List<Org.BouncyCastle.X509.X509Certificate>();
            certificates.Add(certificate);

            var storeParameters = new X509CollectionStoreParameters(certificates);
            IX509Store store = X509StoreFactory.Create("Certificate/Collection", storeParameters);

            generator.AddSigner(privKey, certificate, CmsSignedGenerator.EncryptionRsa, CmsSignedGenerator.DigestSha256);

            generator.AddCertificates(store);

            var message = new CmsProcessableByteArray(bytesToSign);
            CmsSignedData signedData = generator.Generate(message, false);

            return signedData.GetEncoded();
        }

        private byte[] GenerateSignaturePayloadFromKey(TemporaryExposureKeyGatewayDto key)
        {
            MemoryStream memStream = new MemoryStream();
            WriteBytesInByteArray(key.KeyData, memStream);
            WriteSeparatorInArray(memStream);
            WriteBigEndianUInt(key.RollingStartIntervalNumber, memStream);
            WriteSeparatorInArray(memStream);
            WriteBigEndianUInt(key.RollingPeriod, memStream);
            WriteSeparatorInArray(memStream);
            WriteBigEndianInt(key.TransmissionRiskLevel, memStream);
            WriteSeparatorInArray(memStream);
            WriteVisitedCountries(key.VisitedCountries, memStream);
            WriteSeparatorInArray(memStream);
            WriteBase64StringInByteArray(key.Origin, memStream);
            WriteSeparatorInArray(memStream);
            WriteBigEndianInt((int)key.ReportType, memStream);
            WriteSeparatorInArray(memStream);
            WriteBigEndianInt(key.DaysSinceOnsetOfSymptoms, memStream);
            WriteSeparatorInArray(memStream);

            var result = memStream.ToArray();
            return result;
        }

        private void WriteBytesInByteArray(ByteString bytes, MemoryStream memStream)
        {
            var base64 = _encodingService.EncodeToBase64(bytes.ToByteArray());
            WriteStringInByteArray(base64, memStream);
        }

        private void WriteBase64StringInByteArray(string str, MemoryStream memStream)
        {
            var base64 = _encodingService.EncodeToBase64(Encoding.ASCII.GetBytes(str));
            WriteStringInByteArray(base64, memStream);
        }

        private void WriteStringInByteArray(string str, MemoryStream memStream)
        {
            memStream.Write(Encoding.ASCII.GetBytes(str));
        }

        private void WriteSeparatorInArray(MemoryStream memStream)
        {
            memStream.Write(Encoding.ASCII.GetBytes("."));
        }

        private void WriteBigEndianUInt(uint value, MemoryStream memStream)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }
            WriteStringInByteArray(_encodingService.EncodeToBase64(bytes), memStream);
        }

        private void WriteBigEndianInt(int value, MemoryStream memStream)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }
            WriteStringInByteArray(_encodingService.EncodeToBase64(bytes), memStream);
        }

        private void WriteVisitedCountries(Google.Protobuf.Collections.RepeatedField<string> visitedCountries, MemoryStream memStream)
        {
            WriteBase64StringInByteArray(string.Join(",", visitedCountries), memStream);
        }
    }
}
