using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public interface IAnonymousTokenKeySource
    {
        X9ECParameters ECParameters { get; }

        BigInteger GetPrivateKey(string keyId);
    }
}