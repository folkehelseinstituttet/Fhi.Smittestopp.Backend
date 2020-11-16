using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IRsaProviderService
    {
        public RsaSecurityKey GetRsaSecurityKey(string keyId);
    }
}