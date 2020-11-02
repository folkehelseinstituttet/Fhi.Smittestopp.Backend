using Google.Protobuf;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using DIGNDB.App.SmitteStop.Domain.Proto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class ExposureBatchFileUtil
    {
        public const string BinEntryName = "export.bin";
        public const string SigEntryName = "export.sig";
        private string _privateKey;

        public ExposureBatchFileUtil(string certificateThumbprintOrPath)
        {
            _privateKey = GetPrivateKeyFromCertificate(certificateThumbprintOrPath);
        }

        private string GetPrivateKeyFromCertificate(string certificateThumbprintOrPath)
        {
            //when config is a pem-file
            if (certificateThumbprintOrPath.Contains(@".pem"))
            {
                var pem = System.IO.File.ReadAllText(certificateThumbprintOrPath);
                return pem;
            }

            //othwerwise treat as thumbprint for a certstore-cert
            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection =
                    certStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprintOrPath, false);
                if (certCollection.Count > 0)
                {
                    var privateKey = certCollection[0].GetECDsaPrivateKey().ExportECPrivateKey().ToString();
                    return privateKey;
                }
                else
                {
                    throw new SecurityException("Unable to read key from certificate");
                }
            }
        }


        public SignatureInfo[] Signatures => new SignatureInfo[]
        {
    				new SignatureInfo
    				{
      					VerificationKeyId = "238",
      					VerificationKeyVersion = "v1",
      					SignatureAlgorithm = "1.2.840.10045.4.3.2",
      					PrivateKey = _privateKey
    				},
  			};

    		public  async Task<Stream> CreateSignedFileAsync(TemporaryExposureKeyExport export)
    		{
      			export.SignatureInfos.AddRange(Signatures);

      			var ms = new MemoryStream();

      			using (var zipFile = new ZipArchive(ms, ZipArchiveMode.Create, true))
      			using (var bin = await CreateBinAsync(export))
      			using (var sig = CreateSig(export, bin.ToArray()))
      			{
        				// Copy the bin contents into the entry
        				var binEntry = zipFile.CreateEntry(BinEntryName, CompressionLevel.Optimal);
        				using (var binStream = binEntry.Open())
        				{
        					await bin.CopyToAsync(binStream);
        				}

        				// Copy the sig contents into the entry
        				var sigEntry = zipFile.CreateEntry(SigEntryName, CompressionLevel.NoCompression);
        				using (var sigStream = sigEntry.Open())
        				{
        					await sig.CopyToAsync(sigStream);
        				}
      			}

      			// Rewind to the front for the consumer
      			ms.Seek(0, SeekOrigin.Begin);
      			return ms;
    		}

    		public static async Task<MemoryStream> CreateBinAsync(TemporaryExposureKeyExport export)
    		{
      			var stream = new MemoryStream();

      			// Write header
      			await stream.WriteAsync(TemporaryExposureKeyExport.Header);

      			// Write export proto
      			export.WriteTo(stream);

      			// Rewind to the front for the consumer
      			stream.Position = 0;

      			return stream;
    		}

    		public MemoryStream CreateSig(TemporaryExposureKeyExport export, byte[] exportBytes)
    		{
      			var stream = new MemoryStream();

      			// Create signature list object
      			var tk = new TEKSignatureList();
                foreach (var sigInfo in Signatures)
      			{
        				// Generate the signature from the bin file contents
        				var sig = SignData(exportBytes, sigInfo.PrivateKey);

        				tk.Signatures.Add(new TEKSignature
        				{
          					BatchNum = export.BatchNum,
          					BatchSize = export.BatchSize,
          					SignatureInfo = sigInfo,
          					Signature = ByteString.CopyFrom(sig),
        				});
      			}

      			// Write signature proto
      			tk.WriteTo(stream);

      			// Rewind to the front for the consumer
      			stream.Position = 0;

      			return stream;
    		}

    		public static byte[] SignData(byte[] data, string pem)
    		{
      			var reader = new StringReader(pem);
      			PemReader pr = new PemReader(reader);
      			var KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
      			ECPrivateKeyParameters privKey = (ECPrivateKeyParameters)KeyPair.Private;

      			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
      			signer.Init(true, privKey);
      			signer.BlockUpdate(data, 0, data.Length);
      			byte[] sigBytes = signer.GenerateSignature();
      			return sigBytes;
    		}

  	}
}
