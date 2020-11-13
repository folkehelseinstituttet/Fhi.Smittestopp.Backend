using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public interface IJwtTokenReplyAttackService
    {
        void ValidateReplyAttack(SecurityToken token);
    }
}