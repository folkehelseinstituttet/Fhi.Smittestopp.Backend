using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    internal class JwkRsaProviderService : IRsaProviderService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _jwkUrl;

        private RsaSecurityKey _rsaSecurityKey;

        public JwkRsaProviderService(
            HttpMessageHandler httpClientHandler,
            IConfiguration configuration)
        {
            _httpClient = new HttpClient(httpClientHandler);
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            _jwkUrl = new Uri(configuration["JwkUrl"]);
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
            var keys = await _httpClient.GetFromJsonAsync<List<JsonWebKey>>(_jwkUrl);
            if (keys == null)
                throw new RsaPublicKeyNotFoundException(keyId);

            var foundKey = keys.SingleOrDefault(key => key.Kid == keyId);
            if (foundKey == null)
                throw new RsaPublicKeyNotFoundException(keyId);

            return foundKey;
        }

        private RsaSecurityKey CreateRsaSecurityKeyFromJsonWebKey(JsonWebKey jsonWebKey)
        {
            _rsaSecurityKey = new RsaSecurityKey(new RSAParameters
            {
                Exponent = Convert.FromBase64String(jsonWebKey.E),
                Modulus = Convert.FromBase64String(jsonWebKey.N),
            }) {KeyId = jsonWebKey.Kid};

            return _rsaSecurityKey;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}