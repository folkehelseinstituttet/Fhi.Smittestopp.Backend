#nullable enable
using DIGNDB.App.SmitteStop.Core.Contracts;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FederationGatewayApi.Services
{
    public class GatewayHttpClient : IGatewayHttpClient, IDisposable
    {
        private const string FingerprintHeader = "X-SSL-Client-SHA256";
        private const string DistinctiveNameHeader = "X-SSL-Client-DN";
        private const string Accept = "accept";

        private const string CountryHeader = "C=DK";

        private readonly HttpClient _httpClient;

        public GatewayHttpClient(IGatewayKeyProvider gatewayKeyProvider, HttpClientHandler? httpClientHandler = null)
        {

            httpClientHandler ??= new HttpClientHandler();
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ClientCertificates.Add(gatewayKeyProvider.AuthenticationCertificate);

            _httpClient = new HttpClient(httpClientHandler);

            _httpClient.DefaultRequestHeaders.Add(FingerprintHeader, gatewayKeyProvider.AuthenticationCertificateFingerprint);
            _httpClient.DefaultRequestHeaders.Add(DistinctiveNameHeader, CountryHeader);
            _httpClient.DefaultRequestHeaders.Add(Accept, "application / json; version = 1.0");
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent requestBody)
        {
            return await _httpClient.PostAsync(url, requestBody);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            return await _httpClient.SendAsync(message);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}