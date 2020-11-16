using System;

namespace DIGNDB.App.SmitteStop.Core.Exceptions
{
    public class RsaPublicKeyNotFoundException : Exception
    {
        public RsaPublicKeyNotFoundException(string keyId) : base($"Cannot find RSA public key for given key id. Key id: {keyId}")
        {
        }
    }
}