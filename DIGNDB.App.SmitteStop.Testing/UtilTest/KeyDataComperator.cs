using Google.Protobuf;
using System.Collections;

namespace DIGNDB.App.SmitteStop.Testing.UtilTest
{
    public static class KeyDataComperator
    {
        public static bool EqualsKeyData(ByteString protoKeyData, ByteString keyDataToCompare) => EqualsKeyData(protoKeyData.ToByteArray(), keyDataToCompare.ToByteArray());

        public static bool EqualsKeyData(byte[] keyData1, byte[] keyData2) => StructuralComparisons.StructuralEqualityComparer.Equals(keyData1, keyData2);
    }
}
