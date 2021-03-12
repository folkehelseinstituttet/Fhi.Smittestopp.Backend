using AnonymousTokens.Core.Services;
using AnonymousTokens.Server.Protocol;
using DIGNDB.App.SmitteStop.API.Contracts;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class AnonymousTokenValidationService : IAnonymousTokenValidationService
    {
        private readonly IAnonymousTokenKeySource _anonymousTokenKeySource;
        private readonly ITokenVerifier _tokenVerifier;

        public AnonymousTokenValidationService(IAnonymousTokenKeySource anonymousTokenKeySource, ISeedStore seedStore)
        {
            _anonymousTokenKeySource = anonymousTokenKeySource;
            _tokenVerifier = new TokenVerifier(seedStore);
        }

        public async Task<bool> IsTokenValid(string anonymousToken)
        {
            var parts = anonymousToken.Split(".");
            var submittedPoint = _anonymousTokenKeySource.ECParameters.Curve.DecodePoint(Convert.FromBase64String(parts[0]));
            var tokenSeed = Convert.FromBase64String(parts[1]);
            var keyId = parts[2];

            var privateKey = _anonymousTokenKeySource.GetPrivateKey(keyId);

            var isValid = await _tokenVerifier.VerifyTokenAsync(privateKey, _anonymousTokenKeySource.ECParameters.Curve, tokenSeed, submittedPoint);

            return isValid;
        }
    }
}
