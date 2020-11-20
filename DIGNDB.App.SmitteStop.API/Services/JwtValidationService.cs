using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.API.Services
{
    internal class JwtValidationService : IJwtValidationService
    {
        private const string ClientIdClaimType = "client_id";

        private readonly string _validClientIdValue;
        private readonly string _supportedAlgorithm;
        private readonly string _validIssuer;

        private readonly IRsaProviderService _rsaProviderService;
        private readonly IJwtTokenReplyAttackService _jwtTokenReplyAttackService;

        public JwtValidationService(
            IRsaProviderService rsaProviderService,
            IJwtTokenReplyAttackService jwtTokenReplyAttackService,
            JwtAuthorization jwtAuthorizationConfiguration)
        {
            _rsaProviderService = rsaProviderService;
            _jwtTokenReplyAttackService = jwtTokenReplyAttackService;

            _validClientIdValue = jwtAuthorizationConfiguration.JwtValidationRules.ClientId;
            _supportedAlgorithm = jwtAuthorizationConfiguration.JwtValidationRules.SupportedAlgorithm;
            _validIssuer = jwtAuthorizationConfiguration.JwtValidationRules.Issuer;
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            ValidateAlgorithm(jwtToken);

            var validationParameters = GetValidationParameters(jwtToken.Header.Kid);

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken outToken);

            ValidateClientId(outToken);
            _jwtTokenReplyAttackService.ValidateReplyAttack(outToken);

            return true;
        }

        private void ValidateAlgorithm(JwtSecurityToken jwtToken)
        {
            var algorithmInToken = jwtToken.Header.Alg;
            if (algorithmInToken != _supportedAlgorithm)
                throw new NotSupportedException(
                    $"Provided algorithm is not supported. Algorithm in token: {algorithmInToken}. Supported algorithm: {_supportedAlgorithm}");
        }

        private void ValidateClientId(SecurityToken token)
        {
            if (!(token is JwtSecurityToken jwtToken)) return;

            string clientIdInToken = jwtToken.Claims.Single(c => c.Type == ClientIdClaimType).Value;

            var validationResult = clientIdInToken == _validClientIdValue;

            if (!validationResult)
                throw new SecurityTokenException(
                    $"client_id claim is invalid. Expected: {_validClientIdValue}, Got: {clientIdInToken}");
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