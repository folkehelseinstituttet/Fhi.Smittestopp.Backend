using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class AppleService : IAppleService
    {
        private readonly IAppSettingsConfig _appSettingsConfig;
        private readonly ILogger<AppleService> _logger;

        public AppleService(IAppSettingsConfig appSettingsConfig, ILogger<AppleService> logger)
        {
            _appSettingsConfig = appSettingsConfig;
            _logger = logger;
        }

        private string BuildJwtAuthorizationToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var privateKey = LoadPrivateKey();

            var mobileTokenEncrypted = _appSettingsConfig.Configuration.GetValue<string>("appleDeveloperAccount");
            string mobileTokenDecrypted;

            try
            {
                mobileTokenDecrypted = ConfigEncryptionHelper.UnprotectString(mobileTokenEncrypted);
            }
            catch (Exception e)
            {
                _logger.LogError($"Configuration error. Cannot decrypt the mobileToken from configuration. MobileTokenEncrypted: {mobileTokenEncrypted}. Message: {e.Message}");
                throw;
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now,
                Issuer = mobileTokenDecrypted,
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(privateKey), "ES256")
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            string appleKeyIDEncrypted = _appSettingsConfig.Configuration.GetValue<string>("appleKeyID");
            string appleKeyIDDecrypted;
            try
            {
                appleKeyIDDecrypted = ConfigEncryptionHelper.UnprotectString(appleKeyIDEncrypted);
            }
            catch (Exception e)
            {
                _logger.LogError($"Configuration error. Cannot decrypt the appleKeyID from configuration. AppleKeyIDEncrypted: {appleKeyIDEncrypted}. Message: {e.Message}");
                throw;
            }

            token.Header.Add("kid", appleKeyIDDecrypted);
            token.Header.Remove("typ");
            token.Payload.Remove("nbf");
            token.Payload.Remove("exp");
            return tokenHandler.WriteToken(token);
        }

        private ECDsa LoadPrivateKey()
        {
            var decoded =
                ConfigEncryptionHelper.UnprotectString(_appSettingsConfig.Configuration.GetValue<string>("privateKey")).Replace("#", " ").Replace("\r", "");
            using (TextReader reader = new StringReader(decoded))
            {
                var ecPrivateKeyParameters =
                    (ECPrivateKeyParameters)new Org.BouncyCastle.OpenSsl.PemReader(reader).ReadObject();
                var x = ecPrivateKeyParameters.Parameters.G.AffineXCoord.GetEncoded();
                var y = ecPrivateKeyParameters.Parameters.G.AffineYCoord.GetEncoded();
                var d = ecPrivateKeyParameters.D.ToByteArrayUnsigned();
                var msEcp = new ECParameters { Curve = ECCurve.NamedCurves.nistP256, Q = { X = x, Y = y }, D = d };
                return ECDsa.Create(msEcp);
            }
        }

        public AppleQueryBitsDto BuildQueryBitsDto(string deviceToken)
        {
            AppleQueryBitsDto apple = new AppleQueryBitsDto()
            {
                device_token = deviceToken,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                transaction_id = Guid.NewGuid().ToString()
            };
            return apple;
        }

        public AppleUpdateBitsDto BuildUpdateBitsDto(string deviceToken)
        {
            AppleUpdateBitsDto apple = new AppleUpdateBitsDto()
            {
                device_token = deviceToken,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                transaction_id = Guid.NewGuid().ToString(),
                bit0 = false,
                bit1 = false
            };
            return apple;
        }

        public async Task<AppleResponseDto> ExecuteQueryBitsRequest(string deviceToken)
        {
            using (var httpClient = new HttpClient())
            {
                var token = BuildJwtAuthorizationToken();
                var requestUri = new Uri(_appSettingsConfig.Configuration.GetValue<string>("appleQueryBitsUrl"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = await httpClient.PostAsync(requestUri, CreateHttpContent(BuildQueryBitsDto(deviceToken)));
                string content = await request.Content.ReadAsStringAsync();
                AppleResponseDto apple = new AppleResponseDto()
                {
                    ResponseCode = request.StatusCode,
                    Content = content
                };
                return apple;
            }
        }

        public async Task<AppleResponseDto> ExecuteUpdateBitsRequest(string deviceToken)
        {
            using (var httpClient = new HttpClient())
            {
                var token = BuildJwtAuthorizationToken();
                var requestUri = new Uri(_appSettingsConfig.Configuration.GetValue<string>("appleUpdateBitsUrl"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = await httpClient.PostAsync(requestUri, CreateHttpContent(BuildUpdateBitsDto(deviceToken)));
                string content = await request.Content.ReadAsStringAsync();
                AppleResponseDto apple = new AppleResponseDto()
                {
                    ResponseCode = request.StatusCode,
                    Content = content
                };
                return apple;
            }
        }

        private HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        private void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
    }
}
