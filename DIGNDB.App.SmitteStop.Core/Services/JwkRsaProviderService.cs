#nullable enable
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Exceptions;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    internal class JwkRsaProviderService : IRsaProviderService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _jwkUrl;

        private RsaSecurityKey _rsaSecurityKey;

        public JwkRsaProviderService(
            JwtAuthorization jwtAuthorizationConfiguration,
            HttpMessageHandler? httpMessageHandler = null)
        {
            httpMessageHandler ??= new HttpClientHandler();

            _httpClient = new HttpClient(httpMessageHandler);
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            _jwkUrl = new Uri(jwtAuthorizationConfiguration.JwkUrl);
        }

        public RsaSecurityKey GetRsaSecurityKey(string keyId)
        {
            if (_rsaSecurityKey != null)
                return _rsaSecurityKey;

            var jsonWebKey = GetJsonWebKey(keyId).Result;
            var rsaSecurityKey = CreateRsaSecurityKeyFromJsonWebKey(jsonWebKey);

            return rsaSecurityKey;
        }

        private async Task<JsonWebKey> GetJsonWebKey(string keyId)
        {
            var collection = await _httpClient.GetFromJsonAsync<JsonWebKeyCollection>(_jwkUrl);
            if (collection == null)
                throw new RsaPublicKeyNotFoundException(keyId);

            var foundKey = collection.Keys.SingleOrDefault(key => key.Kid == keyId);
            if (foundKey == null)
                throw new RsaPublicKeyNotFoundException(keyId);

            return foundKey;
        }

        private RsaSecurityKey CreateRsaSecurityKeyFromJsonWebKey(JsonWebKey jsonWebKey)
        {
            _rsaSecurityKey = new RsaSecurityKey(new RSAParameters
            {
                Exponent = Base64UrlEncoder.DecodeBytes(jsonWebKey.E),
                Modulus = Base64UrlEncoder.DecodeBytes(jsonWebKey.N),
            }) {KeyId = jsonWebKey.Kid};

            return _rsaSecurityKey;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}