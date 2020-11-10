using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public interface IRsaProviderService
    {
        public RsaSecurityKey GetRsaSecurityKey();
    }
}