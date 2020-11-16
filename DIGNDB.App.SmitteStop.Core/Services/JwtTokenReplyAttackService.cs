using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class JwtTokenReplyAttackService : IJwtTokenReplyAttackService
    {
        private readonly IJwtTokenRepository _jwtTokenRepository;

        public JwtTokenReplyAttackService(IJwtTokenRepository jwtTokenRepository)
        {
            _jwtTokenRepository = jwtTokenRepository;
        }

        public void ValidateReplyAttack(SecurityToken token)
        {
            _jwtTokenRepository.RemoveExpiredTokens();

            var hasTokenBeenUsed = _jwtTokenRepository.HasTokenBeenUsed(token.Id);

            if(hasTokenBeenUsed)
                throw new SecurityTokenReplayDetectedException($"The same token cannot be used again. Token Id: {token.Id}");

            _jwtTokenRepository.InsertValidToken(new JwtToken
            {
                ExpirationTime = token.ValidTo,
                Id = token.Id
            });
        }
    }
}