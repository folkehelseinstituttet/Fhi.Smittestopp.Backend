using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private const string AuthorizedPartyClaimType = "azp";

        private readonly string _validAuthorizedPartyValue;
        private readonly string _supportedAlgorithm;
        private readonly string _validIssuer;

        private readonly IRsaProviderService _rsaProviderService;
        private readonly IJwtTokenReplyAttackService _jwtTokenReplyAttackService;

        public JwtValidationService(
            IRsaProviderService rsaProviderService,
            IJwtTokenReplyAttackService jwtTokenReplyAttackService,
            IConfiguration configuration)
        {
            _rsaProviderService = rsaProviderService;
            _jwtTokenReplyAttackService = jwtTokenReplyAttackService;

            _validAuthorizedPartyValue = configuration[$"{nameof(JwtValidationRules)}:{nameof(JwtValidationRules.AuthorizedPartyValue)}"];
            _supportedAlgorithm = configuration[$"{nameof(JwtValidationRules)}:{nameof(JwtValidationRules.SupportedAlgorithm)}"];
            _validIssuer = configuration[$"{nameof(JwtValidationRules)}:{nameof(JwtValidationRules.Issuer)}"];
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            ValidateAlgorithm(jwtToken);

            var validationParameters = GetValidationParameters(jwtToken.Header.Kid);

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken outToken);

            ValidateAuthorizedParty(outToken);
            _jwtTokenReplyAttackService.ValidateReplyAttack(outToken);

            return true;
        }

        private void ValidateAlgorithm(JwtSecurityToken jwtToken)
        {
            var algorithmInToken = jwtToken.Header.Alg;
            if (algorithmInToken != _supportedAlgorithm)
                throw new NotSupportedException(
                    $"Provided algorithm is not supported. Algorithm in token: {algorithmInToken}. Supported algorithm: {_supportedAlgorithm}");

            string authorizedPartyInToken = jwtToken.Claims.Single(c => c.Type == AuthorizedPartyClaimType).Value;

            var validationResult = authorizedPartyInToken == _validAuthorizedPartyValue;

            if (!validationResult)
                throw new SecurityTokenException(
                    $"AuthorizeParty claim is invalid. Expected: {_validAuthorizedPartyValue}, Got: {authorizedPartyInToken}");
        }

        private void ValidateAuthorizedParty(SecurityToken token)
        {
            if (!(token is JwtSecurityToken jwtToken)) return;

            string authorizedPartyInToken = jwtToken.Claims.Single(c => c.Type == AuthorizedPartyClaimType).Value;

            var validationResult = authorizedPartyInToken == _validAuthorizedPartyValue;

            if (!validationResult)
                throw new SecurityTokenException(
                    $"AuthorizeParty claim is invalid. Expected: {_validAuthorizedPartyValue}, Got: {authorizedPartyInToken}");
        }

        private TokenValidationParameters GetValidationParameters(string rsaKeyId)
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _validIssuer,
                IssuerSigningKey = _rsaProviderService.GetRsaSecurityKey(rsaKeyId),
            };
        }
    }
}