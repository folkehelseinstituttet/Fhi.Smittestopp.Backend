using AnonymousTokens.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class AnonymousTokenSeedStore : ISeedStore
    {
        private readonly IJwtTokenRepository _jwtTokenRepository;
        private readonly AnonymousTokenKeyStoreConfiguration _config;

        public AnonymousTokenSeedStore(IJwtTokenRepository jwtTokenRepository, IOptions<AnonymousTokenKeyStoreConfiguration> config)
        {
            _jwtTokenRepository = jwtTokenRepository;
            _config = config.Value;
        }

        public Task<bool> ExistsAsync(byte[] tokenSeed)
        {
            var tokenId = ToTokenId(tokenSeed);
            var hasBeenUsed = _jwtTokenRepository.HasTokenBeenUsed(tokenId);
            return Task.FromResult(hasBeenUsed);
        }

        public Task<bool> SaveAsync(byte[] tokenSeed)
        {
            var newTokenRecord = new JwtToken
            {
                Id = ToTokenId(tokenSeed),
                // _jwtTokenRepository.RemoveExpiredTokens() assumes DateTime.Now (not .UtcNow)
                ExpirationTime = DateTime.Now + _config.KeyRotationInterval
            };

            _jwtTokenRepository.InsertValidToken(newTokenRecord);

            return Task.FromResult(true);
        }

        private string ToTokenId(byte[] tokenSeed)
        {
            return Convert.ToBase64String(tokenSeed);
        }
    }
}
