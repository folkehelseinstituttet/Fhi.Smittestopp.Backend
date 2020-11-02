using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Models;
using FederationGatewayApi.Services;
using Microsoft.Extensions.DependencyInjection;
using TemporaryExposureKeyGatewayBatchProtoDto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayBatchDto;
using TemporaryExposureKeyGatewayProtoDto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayDto;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Settings;
using System.Security.Cryptography.X509Certificates;
using FederationGatewayApi.Config;
using DIGNDB.App.SmitteStop.Domain;
using System.Net;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class SetupMockedServices
    {
        public ExposureKeyMock _exposureKeyMock { get; set; }
        public WebContextMock _webContextMock { get; set; }
        public CountryMockFactory _countryFactory { get; set; }
        public MockRandomGenerator _rndGenerator { get; set; }

        public SetupMockedServices()
        {
            _exposureKeyMock = new ExposureKeyMock();
            _webContextMock = new WebContextMock();
            _countryFactory = new CountryMockFactory();
            _rndGenerator = new MockRandomGenerator();
        }

        public void SetupWebContextReaderMock(Mock<IGatewayWebContextReader> gatewayContextReader)
        {
            gatewayContextReader.Setup(mock => mock.GetItemsFromRequest(It.IsAny<string>())).Returns((string response) =>
            {
                if (response == null)
                    return new List<TemporaryExposureKeyGatewayDto>();
                else
                    return _exposureKeyMock.MockListOfTemporaryExposureKeyDto();
            });

            gatewayContextReader.Setup(mock => mock.ReadHttpContextStream(It.IsAny<HttpResponseMessage>())).Returns((HttpResponseMessage responseMessage) =>
            {

                if (responseMessage == null || responseMessage.RequestMessage == null)
                    throw new Exception();
                else
                    return _webContextMock.MockValidBodyJSON();
            });
        }
        public void SetupWebContextReaderMockWithBadContext(Mock<IGatewayWebContextReader> gatewayContextReader)
        {
            gatewayContextReader.Setup(mock => mock.GetItemsFromRequest(It.IsAny<string>())).Returns((string response) =>
            {
                throw new Exception();
            });
        }

        public void SetupKeyFilterMock(Mock<IKeyFilter> keyFilter)
        {
            IList<string> errorMessageList = new List<string>();
            keyFilter.Setup(mock => mock.MapKeys(It.IsAny<IList<TemporaryExposureKeyGatewayDto>>())).Returns((IList<TemporaryExposureKeyGatewayDto> keys) =>
            {
                return _exposureKeyMock.MockListOfTemporaryExposureKeys();
            });

            keyFilter.Setup(mock => mock.RemoveKeyDuplicatesAsync(It.IsAny<IList<TemporaryExposureKey>>())).Returns((IList<TemporaryExposureKey> keys) =>
            {
                var keyData = _exposureKeyMock.MockListOfTemporaryExposureKeys();
                keys.RemoveAt(2);
                keys.RemoveAt(3);
                return Task.FromResult(keys);
            });

            keyFilter.Setup(mock => mock.ValidateKeys(It.IsAny<IList<TemporaryExposureKey>>(), out errorMessageList)).Returns((IList<TemporaryExposureKey> keys, IList<string> errorMessageList) =>
            {
                return _exposureKeyMock.MockListOfTemporaryExposureKeys();
            });

        }

        public void SetupEpochConverterMock(Mock<IEpochConverter> epochConverter)
        {
            epochConverter.Setup(mock => mock.ConvertToEpoch(It.IsAny<DateTime>())).Returns((DateTime date) =>
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64((date - epoch).TotalSeconds);
            });
            epochConverter.Setup(mock => mock.ConvertFromEpoch(It.IsAny<long>())).Returns((long epochTime) =>
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddSeconds(epochTime);
            });
        }

        public void SetupTemopraryExposureKeyRepositoryMock(Mock<ITemporaryExposureKeyRepository> tempKeyRepository)
        {
            tempKeyRepository.Setup(mock => mock.AddTemporaryExposureKeys(It.IsAny<List<TemporaryExposureKey>>())).Returns((List<TemporaryExposureKey> keys) =>
            {
                return Task.CompletedTask;
            });

            List<TemporaryExposureKey> keys = new List<TemporaryExposureKey>();
            for (int i = 0; i < _exposureKeyMock.MockListLength - 2; i++)
            {
                keys.Add(_exposureKeyMock.MockValidKey());
            }
            keys.Add(ExposureKeyMock._potentialDuplicate01);
            keys.Add(ExposureKeyMock._potentialDuplicate02);

            tempKeyRepository.Setup(mock => mock.GetAllKeysNextBatch(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int numberOfRecordsToSkip, int batchSize) => keys);
            tempKeyRepository.Setup(mock => mock.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => Task.Run(() => keys as IList<TemporaryExposureKey>));
        }

        public void SetupSignatureServiceMock(Mock<ISignatureService> signatureService)
        {

            signatureService.Setup(mock => mock.Sign(It.IsAny<TemporaryExposureKeyGatewayBatchProtoDto>(), It.IsAny<SortOrder>())).Returns((TemporaryExposureKeyGatewayBatchProtoDto protoKeyBatch, SortOrder sortOrder) =>
            {
                return _rndGenerator.GenerateKeyData(16);
            });

        }
        public void SetupEncodingServiceMock(Mock<IEncodingService> encodingService)
        {

            encodingService.Setup(mock => mock.DecodeFromBase64(It.IsAny<string>())).Returns((string encodedData) =>
            {
                if (encodedData == null) return null;
                byte[] encodedBytes = Convert.FromBase64String(encodedData);
                return System.Text.Encoding.UTF8.GetString(encodedBytes);
            });
            encodingService.Setup(mock => mock.EncodeToBase64(It.IsAny<string>())).Returns((string plainText) =>
            {
                if (plainText == null) return null;
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return plainTextBytes == null ? null : Convert.ToBase64String(plainTextBytes);
            });
            encodingService.Setup(mock => mock.EncodeToBase64(It.IsAny<byte[]>())).Returns((byte[] textBytes) =>
            {
                return textBytes == null ? null : Convert.ToBase64String(textBytes);
            });

        }


        public void SetupSettingsServiceMock(Mock<ISettingsService> settingsService)
        {

            settingsService.Setup(mock => mock.GetGatewayUploadState());
            settingsService.Setup(mock => mock.SaveGatewaySyncState(It.IsAny<GatewayUploadState>()));

        }

        public void SetupStoreServiceMock(Mock<IEFGSKeyStoreService> storeService)
        {
            storeService.Setup(mock => mock.FilterAndSaveKeys(It.IsAny<IList<TemporaryExposureKeyGatewayDto>>()));
        }

        public void SetupKeyValidatorMock(Mock<IKeyValidator> keyValidator)
        {
            var errorMessage = String.Empty;
            keyValidator.Setup(mock => mock.ValidateKeyAPI(It.IsAny<TemporaryExposureKey>(), out errorMessage)).Returns((TemporaryExposureKey key, string errorMessage) =>
            {
                if (key.KeyData.Length == 16)
                    return true;
                else
                    return false;
            });
            keyValidator.Setup(mock => mock.ValidateKeyGateway(It.IsAny<TemporaryExposureKey>(), out errorMessage)).Returns((TemporaryExposureKey key, string errorMessage) =>
            {
                if (key.Origin != _countryFactory.GenerateCountry(7, "DK") && key.KeyData.Length == 16)
                    return true;
                else
                    return false;
            });

        }

        public void SetupHttpGatewayClientMock(Mock<IGatewayHttpClient> client)
        {
            var empty = true;
            client.Setup(mock => mock.SendAsync(It.IsAny<HttpRequestMessage>())).Returns((HttpRequestMessage message) =>
            {
                var header = message.Headers.GetValues("batchTag").SingleOrDefault();
                var endBatchTagValue =  $"{DateTime.UtcNow.Date.ToString("yyyy-MM-dd").Replace("-", string.Empty)}-{2}";
                var responseMessage = new HttpResponseMessage();

                if (header == endBatchTagValue)
                {
                    responseMessage = _webContextMock.MockHttpResponse(empty);
                }
                else
                {
                    responseMessage = _webContextMock.MockHttpResponse();

                }
                return Task.FromResult(responseMessage);
            });
            client.Setup(mock => mock.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(_webContextMock.MockHttpResponse()));
        }

        public void SetupMapperAndCountryRepositoryMock(Mock<ICountryRepository> countryRepository)
        {
            var denmark = new Country() { Id = 2, Code = "DK" };
            var germany = new Country() { Id = 3, Code = "DE" };
            var netherlands = new Country() { Id = 4, Code = "NL" };
            var NonExistingCountryCode = "XY";


            countryRepository
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code == "NL")))
                .Returns(netherlands);
            countryRepository
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code == "DE")))
                .Returns(germany);
            countryRepository
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code == "DK")))
                .Returns(denmark);

            countryRepository
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code != "DE" && code != "NL" && code != "DK")))
                .Returns(new Country() { Id = 0, Code = NonExistingCountryCode });

            var mockedVisitedCountries = new List<Country>
            {
                denmark,
                germany,
                netherlands
            };

            countryRepository
                .Setup(repo => repo.FindByIsoCodes(It.IsAny<IList<string>>()))
                .Returns(
                    (IList<string> param) => mockedVisitedCountries.Where(country => param.Contains(country.Code))
                );


        }

        public  IMapper CreateAutoMapperWithDependencies(ICountryRepository repository)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();

            services.AddSingleton(repository);
            services.AddAutoMapper(typeof(TemporaryExposureKeyToEuGatewayMapper));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMapper>();

        }

        public EuGatewayConfig CreateEuGatewayConfig()
        {
            var euGatewayConfig = new EuGatewayConfig();
            euGatewayConfig.Url = "https://acc-efgs-ws.tech.ec.europa.eu/";
            euGatewayConfig.AuthenticationCertificateFingerprint = "A3C3E533CC9FEACA026F99F688F4488B5FC16BD0E6A80E6E0FC03760983DBF3F";
            euGatewayConfig.SigningCertificateFingerprint = "979673B55DB0B7E2B35B12CF2A342655F059314BC46323C43BCD3BFC82374BFB";
            return euGatewayConfig;
        }

        private void AddErrorList(IList<string> errorList)
        {
            errorList.Add("Bad error!");
            errorList.Add("Scary error!");
            errorList.Add("EndOfTheWorld error!");
        }
    }
}
