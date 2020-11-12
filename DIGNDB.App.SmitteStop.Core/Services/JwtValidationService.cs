using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private const string AuthorizedPartyClaimType = "azp";
        private const string ValidAuthorizedPartyValue = "smittestopp";
        private const string SupportedAlgorithm = "RS256";

        private readonly IRsaProviderService _rsaProviderService;
        private readonly IJwtTokenReplyAttackService _jwtTokenReplyAttackService;

        public JwtValidationService(
            IRsaProviderService rsaProviderService,
            IJwtTokenReplyAttackService jwtTokenReplyAttackService)
        {
            _rsaProviderService = rsaProviderService;
            _jwtTokenReplyAttackService = jwtTokenReplyAttackService;
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            ValidateAlgorithm(jwtToken);
            var validationParameters = GetValidationParameters(jwtToken.Header.Kid);

            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken outToken);

            ValidateAuthorizedParty(outToken);
            _jwtTokenReplyAttackService.ValidateReplyAttack(outToken);

            return true;
        }

        private void ValidateAlgorithm(JwtSecurityToken jwtToken)
        {
            var algorithmInToken = jwtToken.Header.Alg;
            if (algorithmInToken != SupportedAlgorithm)
                throw new NotSupportedException(
                    $"Provided algorithm is not supported. Algorithm in token: {algorithmInToken}. Supported algorithm: {SupportedAlgorithm}");

            string authorizedPartyInToken = jwtToken.Claims.Single(c => c.Type == AuthorizedPartyClaimType).Value;

            var validationResult = authorizedPartyInToken == ValidAuthorizedPartyValue;

            if (!validationResult)
                throw new SecurityTokenException(
                    $"AuthorizeParty claim is invalid. Expected: {ValidAuthorizedPartyValue}, Got: {authorizedPartyInToken}");
        }

        private void ValidateAuthorizedParty(SecurityToken token)
        {
            if (!(token is JwtSecurityToken jwtToken)) return;

            string authorizedPartyInToken = jwtToken.Claims.Single(c => c.Type == AuthorizedPartyClaimType).Value;

            var validationResult = authorizedPartyInToken == ValidAuthorizedPartyValue;

            if (!validationResult)
                throw new SecurityTokenException(
                    $"AuthorizeParty claim is invalid. Expected: {ValidAuthorizedPartyValue}, Got: {authorizedPartyInToken}");
        }

        private TokenValidationParameters GetValidationParameters(string rsaKeyId)
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = "https://dev-smittestopp-verification.azurewebsites.net",
                IssuerSigningKey = _rsaProviderService.GetRsaSecurityKey(rsaKeyId),
            };
        }
    }
}