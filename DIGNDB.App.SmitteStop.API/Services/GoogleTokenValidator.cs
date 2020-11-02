using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public static class GoogleTokenValidator
    {
        /// <summary>
        /// Parses and verifies a SafetyNet attestation statement.
        /// </summary>
        /// <param name="signedAttestationStatement">A string containing the
        /// JWT attestation statement.</param>
        /// <returns>A parsed attestation statement. null if the statement is
        /// invalid.</returns>
        public static AttestationStatement ParseAndVerify(
            string signedAttestationStatement, ILogger _logger)
        {
            // First parse the token and get the embedded keys.
            JwtSecurityToken token;
            token = new JwtSecurityToken(signedAttestationStatement);
            // We just want to validate the authenticity of the certificate.
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = GetEmbeddedKeys(token)
            };

            // Perform the validation
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(
                    signedAttestationStatement,
                    validationParameters,
                    out validatedToken);

            // Verify the hostname
            if (!(validatedToken.SigningKey is X509SecurityKey))
            {
                // The signing key is invalid.
                throw new ArgumentException("The signing key is not X509SecurityKey");
            }
            if (!VerifyHostname(
                "attest.android.com",
                validatedToken.SigningKey as X509SecurityKey, _logger))
            {
                // The certificate isn't issued for the hostname
                // attest.android.com.
                throw new ArgumentException("The certificate isn't issued for the correct hostname");
            }

            // Parse and use the data JSON.
            Dictionary<string, string> claimsDictionary = token.Claims
                .ToDictionary(x => x.Type, x => x.Value);

            return new AttestationStatement(claimsDictionary);
        }

        /// <summary>
        /// Verifes an X509Security key, and checks that it is issued for a
        /// given hostname.
        /// </summary>
        /// <param name="hostname">The hostname to check to.</param>
        /// <param name="securityKey">The security key to verify.</param>
        /// <returns>true if securityKey is valid and is issued to the given
        /// hostname.</returns>
        private static bool VerifyHostname(
            string hostname,
            X509SecurityKey securityKey, ILogger logger)
        {
            string commonName;
            // Verify the certificate with Verify(). Alternatively, you
            // could use the commented code below instead of Verify(), to
            // get more details of why a particular verification failed.
            //
            var chain = new X509Chain();
            var chainBuilt = chain.Build(securityKey.Certificate);
            StringBuilder s = null;
            if (!chainBuilt)
            {
                s = new StringBuilder(); // One could use a StringBuilder instead.
                foreach (X509ChainStatus chainStatus in
                    chain.ChainStatus)
                {
                    s.Append(string.Format(
                        "Chain error: {0} {1}\n",
                        chainStatus.Status,
                        chainStatus.StatusInformation));
                }
                throw new ArgumentException("Error verifying certificate:" + s?.ToString());
            }

            commonName = securityKey.Certificate.GetNameInfo(
                X509NameType.DnsName, false);
            if (commonName != hostname)
            {
                throw new ArgumentException($"Commonname:{commonName} is different than hostname:{hostname}");
            }
            return true;
        }

        /// <summary>
        /// Retrieves the X509 security keys embedded in a JwtSecurityToken.
        /// </summary>
        /// <param name="token">The token where the keys are to be retrieved
        /// from.</param>
        /// <returns>The embedded security keys. null if there are no keys in
        /// the security token.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the JWT data
        /// does not contain a valid signature
        /// header "x5c".</exception>
        /// <exception cref="CryptographicException">Thrwon when the JWT data
        /// does not contain valid signing
        /// keys.</exception>
        private static X509SecurityKey[] GetEmbeddedKeys(
            JwtSecurityToken token)
        {
            // The certificates are embedded in the "x5c" part of the header.
            // We extract them, parse them, and then create X509 keys out of
            // them.
            X509SecurityKey[] keys = null;
            keys = (token.Header["x5c"] as IEnumerable)
                .Cast<object>()
                .Select(x => x.ToString())
                .Select(x => new X509SecurityKey(
                    new X509Certificate2(Convert.FromBase64String(x))))
                .ToArray();
            return keys;
        }
    }
}
