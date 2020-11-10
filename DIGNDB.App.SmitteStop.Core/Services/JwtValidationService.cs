using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private const string AuthorizedPartyClaimType = "azp";
        private const string ValidAuthorizedPartyValue = "smittestopp";

        private readonly IRsaProviderService _rsaProviderService;

        public JwtValidationService(IRsaProviderService rsaProviderService)
        {
            _rsaProviderService = rsaProviderService;
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken outToken);

            ValidateAuthorizedParty(outToken);

            return true;
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

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = "https://dev-smittestopp-verification.azurewebsites.net",
                IssuerSigningKey = _rsaProviderService.GetRsaSecurityKey(),
            };
        }

        static byte[] FromBase64Url(string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                .Replace("-", "+");
            return Convert.FromBase64String(base64);
        }
    }
}