using System.Text;
using Google.Protobuf;

namespace DIGNDB.App.SmitteStop.Domain.Proto
{
    public partial class TemporaryExposureKeyExport
  	{
    		public const int MaxKeysPerFile = 750000;

    		public const string HeaderContents = "EK Export v1    ";

    		public static readonly byte[] Header = Encoding.UTF8.GetBytes(HeaderContents);
  	}

  	public partial class TemporaryExposureKey
  	{
    		public TemporaryExposureKey(byte[] keyData, int rollingStart, int rollingDuration, int transmissionRisk)
    		{
      			KeyData = ByteString.CopyFrom(keyData);
      			RollingStartIntervalNumber = rollingStart;
      			RollingPeriod = rollingDuration;
      			TransmissionRiskLevel = transmissionRisk;
    		}
  	}

  	public partial class SignatureInfo
  	{
    		public string PrivateKey { get; set; }
  	}
}
