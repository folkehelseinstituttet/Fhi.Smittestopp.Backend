using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IJwtTokenReplyAttackService
    {
        void ValidateReplyAttack(SecurityToken token);
    }
}