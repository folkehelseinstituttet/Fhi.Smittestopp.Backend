using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IRsaProviderService
    {
        public RsaSecurityKey GetRsaSecurityKey(string keyId);
    }
}