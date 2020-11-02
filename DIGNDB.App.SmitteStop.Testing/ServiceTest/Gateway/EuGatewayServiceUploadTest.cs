using AutoMapper;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Enums;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Config;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Services;
using FederationGatewayApi.Services.Settings;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TemporaryExposureKeyGatewayBatchProtoDto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayBatchDto;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class EuGatewayServiceUploadTest
    {
        private DigNDB_SmittestopContext _dbContext;
        private EuGatewayConfig _config;

        private Country _denmark;
        private Country _poland;
        private Country _germany;
        private Country _latvia;

        [SetUp]
        public void CreateDataSet()
        {
            var options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                                .UseInMemoryDatabase(nameof(EuGatewayServiceUploadTest))
                                .Options;

            _dbContext = new DigNDB_SmittestopContext(options);
            _dbContext.Database.EnsureDeleted();

            _config = new EuGatewayConfig()
            {
                AuthenticationCertificateFingerprint = "AuthenticationCertificateFingerprint",
                SigningCertificateFingerprint = "SigningCertificateFingerprint",
                Url = "http://netcompany.pl",
            };

            _denmark = TestCountryBuilder.Denmark.Build();
            _poland = TestCountryBuilder.Poland.Build();
            _germany = TestCountryBuilder.Germany.Build();
            _latvia = TestCountryBuilder.Latvia.Build();

            _dbContext.AddRange(new Country[] { _denmark, _poland, _germany, _latvia });
            _dbContext.SaveChanges();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(100000)]
        [TestCase(750001)]
        public void UploadKeysInMultipleBatches_shouldSendOnlyDkKeys(int batchSize)
        {
            // .: Setup
            // key data must be unique for verification methods
            var denmarkKeys_ApiV1 = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion1)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-1))
                .Build(new[] { "Dk_V1_1", "Dk_V1_2", "Dk_V1_3", "Dk_V1_4" });

            var denmarkKeys_ApiV2 = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-1))
                .SetDaysSinceOnsetOfSymptoms(10)
                .Build(new[] { "Dk_V2_1", "Dk_V2_2", "Dk_V2_3", "Dk_V2_4" });

            var denmarkKeys_ApiV2_InvalidDaysSinceOnsetOfSymptoms = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-1))
                .SetDaysSinceOnsetOfSymptoms(100)
                .Build(new[] { "Dk_V2_5", "Dk_V2_6", "Dk_V2_7", "Dk_V2_8" });

            var denmarkKeys_ApiV2_Old = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-16))
                .Build(new[] { "Dk_V2_Old_1", "Dk_V2_Old_2", "Dk_V2_Old_3", "Dk_V2_Old_4" });

            var germanKeys_FromGateway = TestTemporaryExposureKeyBuilder.CreateDefault(_germany)
                .SetVisitedCountries(new[] { _poland })
                .SetKeySource(KeySource.Gateway)
                .Build(new[] { "De_Gate_1", "De_Gate_2", "De_Gate_3", "De_Gate_4", "De_Gate_5", "De_Gate_6" });

            var latviaKeys_FromGateway = TestTemporaryExposureKeyBuilder.CreateDefault(_latvia)
                .SetKeySource(KeySource.Gateway)
                .Build(new[] { "Lv_Gate_1", "Lv_Gate_2", "Lv_Gate_3", "Lv_Gate_4", "Lv_Gate_5", "Lv_Gate_6" });

            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_ApiV1);
            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_ApiV2);
            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_ApiV2_Old);
            _dbContext.TemporaryExposureKey.AddRange(germanKeys_FromGateway);
            _dbContext.TemporaryExposureKey.AddRange(latviaKeys_FromGateway);
            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_ApiV2_InvalidDaysSinceOnsetOfSymptoms);
            _dbContext.SaveChanges();

            // setup mock
            var gatewayHttpClientMock = new Mock<IGatewayHttpClient>();

            var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Content = new StringContent("") };
            gatewayHttpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                 .ReturnsAsync(expectedResponse);

            var service = CreateGatewayServiceAndDependencies(gatewayHttpClientMock.Object);

            // .: Act
            service.UploadKeysToTheGateway(10, batchSize);

            // .: Verify
            var requestArgInterceptor = new ArgumentInterceptor<HttpContent>();
            gatewayHttpClientMock.Verify(c => c.PostAsync(It.IsAny<string>(), requestArgInterceptor.CreateCaptureAction()));

            var allBatchesPassedToHttpClient = ParseRequestBodiesBatches(requestArgInterceptor);
            var keysFromAllSentBatches = allBatchesPassedToHttpClient.SelectMany(bath => bath.Keys).ToList();

            keysFromAllSentBatches.Should()
                .OnlyContain(key => key.Origin == _denmark.Code, because: "Only DK keys from APIV2 can be send to the UE Gateway.")
                .And.OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key => denmarkKeys_ApiV2.Any(keyApiv2 => EqualsKeyData(key.KeyData.ToByteArray(), keyApiv2.KeyData)));

            int expectedNumberOfBatches = (int)Math.Ceiling(denmarkKeys_ApiV2.Count / (decimal)batchSize);
            allBatchesPassedToHttpClient.Should()
               .NotBeNull()
               .And.HaveCount(expectedNumberOfBatches, because: "Keys needed to be send in batches");

            allBatchesPassedToHttpClient.ToList().ForEach(
                  batch =>
                  {
                      batch.Keys.Should()
                      .HaveCountGreaterThan(0, because: "Service cannot send empty requests.")
                      .And.HaveCountLessOrEqualTo(batchSize, because: "Service cannot send batch with wrong size.")
                      .And.OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.");
                  });
        }

        [TestCase]

        public void UploadKeysInMultipleBatches_connectionErrrorOccured_shouldSendAllDkKeys()
        {
            int batchSize = 2;
            int keysNotOlderThenDays = 14;
            // .: Setup - key data must be unique for verification methods
            var keysThatShouldBeSent = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-1))
                .Build(new[] { "Dk_V2_1", "Dk_V2_2", "Dk_V2_3", "Dk_V2_4" });

            var denmarkKeys_ApiV2_Old = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetVisitedCountries(new[] { _germany, _poland })
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-16))
                .Build(new[] { "Dk_V2_Old_1", "Dk_V2_Old_2", "Dk_V2_Old_3", "Dk_V2_Old_4" });

            var germanKeys_FromGateway = TestTemporaryExposureKeyBuilder.CreateDefault(_germany)
                .SetVisitedCountries(new[] { _poland })
                .SetKeySource(KeySource.Gateway)
                .Build(new[] { "De_Gate_1", "De_Gate_2", "De_Gate_3", "De_Gate_4", "De_Gate_5", "De_Gate_6" });

            _dbContext.TemporaryExposureKey.AddRange(keysThatShouldBeSent);
            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_ApiV2_Old);
            _dbContext.TemporaryExposureKey.AddRange(germanKeys_FromGateway);
            _dbContext.SaveChanges();

            // setup mock
            var gatewayHttpClientMock = new Mock<IGatewayHttpClient>();

            var successfulResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Content = new StringContent("") };
            var errorResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("") };

            gatewayHttpClientMock.SetupSequence(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                 .ReturnsAsync(successfulResponse)
                                 .ReturnsAsync(errorResponse)
                                 .ReturnsAsync(errorResponse)
                                 .ReturnsAsync(successfulResponse);

            var service = CreateGatewayServiceAndDependencies(gatewayHttpClientMock.Object);

            // .: Act
            try
            {
                service.UploadKeysToTheGateway(keysNotOlderThenDays, batchSize); // partial success - one batch sent, second fail
            }
            catch (Exception) { }

            // emulate Job retry
            try
            {
                service.UploadKeysToTheGateway(keysNotOlderThenDays, batchSize);  // first batch fails
            }
            catch (Exception) { }


            // emulate Job retry
            service.UploadKeysToTheGateway(keysNotOlderThenDays, batchSize); // success
            // .: Verify
            var requestArgInterceptor = new ArgumentInterceptor<HttpContent>();
            gatewayHttpClientMock.Verify(c => c.PostAsync(It.IsAny<string>(), requestArgInterceptor.CreateCaptureAction()));

            var allBatchesPassedToHttpClient = ParseRequestBodiesBatches(requestArgInterceptor);

            var firstSucessfulCall = allBatchesPassedToHttpClient[0].Keys;
            var keysFromRetry1 = allBatchesPassedToHttpClient[1].Keys;
            var keysFromRetry2 = allBatchesPassedToHttpClient[2].Keys;
            var lastSuccessfulCall = allBatchesPassedToHttpClient[3].Keys;
            var keysFromAllSuccessfulCalls = firstSucessfulCall.Concat(lastSuccessfulCall);

            // all  keys has been sent
            keysFromAllSuccessfulCalls.Should()
                .OnlyContain(key => key.Origin == _denmark.Code, because: "Only DK keys from APIV2 can be send to the UE Gateway.")
                .And.OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key => keysThatShouldBeSent.Any(keyApiv2 => EqualsKeyData(key.KeyData.ToByteArray(), keyApiv2.KeyData)))
                .And.HaveCount(keysThatShouldBeSent.Count);

            // Service was trying to send  keys again
            keysFromRetry1.Should()
                .OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key2Check => keysThatShouldBeSent.Any(keyFromCollection => EqualsKeyData(key2Check.KeyData.ToByteArray(), keyFromCollection.KeyData)))
                .And.NotContain(key2Check => firstSucessfulCall.Any(keyFromCollection => EqualsKeyData(key2Check.KeyData, keyFromCollection.KeyData)));

            keysFromRetry2.Should()
                .OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key2Check => keysFromRetry1.Any(denmarkKey => EqualsKeyData(key2Check.KeyData, denmarkKey.KeyData)))
                .And.HaveCount(keysFromRetry1.Count);

            lastSuccessfulCall.Should()
                .OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key => keysThatShouldBeSent.Any(keyApiv2 => EqualsKeyData(key.KeyData.ToByteArray(), keyApiv2.KeyData)), because: "Can contain only keys that should be sent.")
                .And.OnlyContain(key2Check => keysFromRetry1.Any(keyFromCollection => EqualsKeyData(key2Check.KeyData, keyFromCollection.KeyData)), because: "Service should try to sent the same keys again.")
                .And.OnlyContain(key2Check => keysFromRetry2.Any(keyFromCollection => EqualsKeyData(key2Check.KeyData, keyFromCollection.KeyData)), because: "Service should try to sent the same keys again.")
                .And.NotContain(key2Check => firstSucessfulCall.Any(keyFromCollection => EqualsKeyData(key2Check.KeyData, keyFromCollection.KeyData)), because: "Service cannot send duplicates.")
                .And.HaveCount(keysFromRetry1.Count)
                .And.HaveCount(keysFromRetry2.Count);

            allBatchesPassedToHttpClient.ToList().ForEach(
                  batch =>
                  {
                      batch.Keys.Should()
                      .HaveCountGreaterThan(0, because: "Service cannot send empty requests.")
                      .And.HaveCountLessOrEqualTo(batchSize, because: "Service cannot send batch with wrong size.");
                  });
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(100000)]
        [TestCase(750001)]
        public void UploadKeysInMultipleBatchesAndMultipleCalls_shouldSendAllDkKeys(int batchSize)
        {
            int keysNotOlderThenDays = 14;
            // .: Setup - key data must be unique for verification methods
            var dkApiV2DefaultBuilder = TestTemporaryExposureKeyBuilder.CreateDefault(_denmark)
                .SetKeySource(KeySource.SmitteStopApiVersion2)
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-1));

            var otherKeysDefaultBuilder = TestTemporaryExposureKeyBuilder.CreateDefault(_poland)
               .SetKeySource(KeySource.Gateway)
               .SetRollingStartNumber(DateTime.UtcNow.AddDays(-3));

            // data for upload 1
            // it is not possible to have creation date exactly the same as before for data that appear in the db later
            dkApiV2DefaultBuilder.SetCreatedOn(DateTime.Now);
            var denmarkKeys_upload1 = dkApiV2DefaultBuilder.Copy()
                .SetVisitedCountries(new[] { _germany, _poland })
                .Build(new[] { "Dk_U1_1", "Dk_U1_2", "Dk_U1_3", "Dk_U1_4" });

            var otherKeys_upload1 = otherKeysDefaultBuilder.Copy()
                .SetOrigin(_poland)
                .Build(new[] { "Other_U1_1", "Other_U1_2", "Other_U1_2", "Other_U1_3" });

            // data for upload 2
            dkApiV2DefaultBuilder.SetCreatedOn(DateTime.Now);
            var denmarkKeys1_U2 = dkApiV2DefaultBuilder.Copy()
                .SetVisitedCountries(new[] { _germany })
                .Build(new[] { "Dk_U2_1_1", "Dk_U2_1_2", "Dk_U2_1_3", "Dk_U2_1_4" });

            // keys created before keys from upload 1 but uploaded later to the SmitteBackend (rollingStartNumber is lower but CreatedOn is later)
            var denmarkKeys2_U2 = dkApiV2DefaultBuilder.Copy()
                .SetRollingStartNumber(DateTime.UtcNow.AddDays(-3))
                .Build(new[] { "Dk_U2_2_1", "Dk_U2_2_2", "Dk_U2_2_3", "Dk_U2_2_4" });

            var denmarkKeys_upload2 = denmarkKeys1_U2.Concat(denmarkKeys2_U2).ToList();

            var otherKeys_upload2 = otherKeysDefaultBuilder
               .Build(new[] { "Other_U2_1_1", "Other_U2_1_2", "Other_U2_1_3", "Other_U2_1_4" });

            var otherKeys2_upload2 = otherKeysDefaultBuilder.Copy()
              .SetOrigin(_latvia)
              .Build(new[] { "Other_U2_2_1", "Other_U2_2_2", "Other_U2_2_3", "Other_U2_2_4" });

            // setup mock
            var gatewayHttpClientMock = new Mock<IGatewayHttpClient>();

            var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Content = new StringContent("") };
            gatewayHttpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
            .ReturnsAsync(expectedResponse);

            var service = CreateGatewayServiceAndDependencies(gatewayHttpClientMock.Object);

            // .: Act
            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_upload1);
            _dbContext.TemporaryExposureKey.AddRange(otherKeys_upload1);
            _dbContext.SaveChanges();

            service.UploadKeysToTheGateway(keysNotOlderThenDays, batchSize);

            _dbContext.TemporaryExposureKey.AddRange(denmarkKeys_upload2);
            _dbContext.TemporaryExposureKey.AddRange(otherKeys_upload2);
            _dbContext.TemporaryExposureKey.AddRange(otherKeys2_upload2);
            _dbContext.SaveChanges();

            service.UploadKeysToTheGateway(keysNotOlderThenDays, batchSize);

            // .: Verify
            // get data
            var requestArgInterceptor = new ArgumentInterceptor<HttpContent>();
            gatewayHttpClientMock.Verify(c => c.PostAsync(It.IsAny<string>(), requestArgInterceptor.CreateCaptureAction()));

            var allBatchesPassedToHttpClient = ParseRequestBodiesBatches(requestArgInterceptor);
            var keysFromAllSentBatches = allBatchesPassedToHttpClient.SelectMany(bath => bath.Keys).ToList();
            // assert
            var expected_AllSentKeys = denmarkKeys_upload1.Concat(denmarkKeys_upload2);
            int expectedNumberOfBatchesInUpload1 = (int)Math.Ceiling(denmarkKeys_upload1.Count / (decimal)batchSize);
            int expectedNumberOfBatchesInUpload2 = (int)Math.Ceiling(denmarkKeys_upload2.Count / (decimal)batchSize);

            keysFromAllSentBatches.Should()
             .OnlyContain(key => key.Origin == _denmark.Code, because: "Only DK keys from APIV2 can be send to the UE Gateway.")
             .And.HaveCount(expected_AllSentKeys.Count(), "Service need to send all keys valid for the sending.")
             .And.OnlyHaveUniqueItems(key => key.KeyData.ToBase64(), because: "Service cannot send duplicates.")
             .And.OnlyContain(key => expected_AllSentKeys.Any(keyApiv2 => EqualsKeyData(key.KeyData.ToByteArray(), keyApiv2.KeyData)));

            allBatchesPassedToHttpClient.Should()
               .NotBeNull()
               .And.HaveCount(expectedNumberOfBatchesInUpload1 + expectedNumberOfBatchesInUpload2, because: "Keys needed to be send in batches");

            allBatchesPassedToHttpClient.ToList().ForEach(
                  batch =>
                  {
                      batch.Keys.Should()
                      .HaveCountGreaterThan(0, because: "Service cannot send empty requests.")
                      .And.HaveCountLessOrEqualTo(batchSize, because: "Service cannot send batch with wrong size.")
                      .And.OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.");
                  });
        }

        #region Helpers
        private EuGatewayService CreateGatewayServiceAndDependencies(IGatewayHttpClient httpClient)
        {
            var translationsRepositoryMock = new Mock<IGenericRepository<Translation>>(MockBehavior.Strict);
            var countryRepository = new CountryRepository(_dbContext, translationsRepositoryMock.Object);
            var keysRepository = new TemporaryExposureKeyRepository(_dbContext, countryRepository);

            var signatureServiceMock = new Mock<ISignatureService>(MockBehavior.Strict);
            signatureServiceMock.Setup(sigService => sigService.Sign(It.IsAny<TemporaryExposureKeyGatewayBatchProtoDto>(), Domain.SortOrder.ASC))
                .Returns(new byte[] { 1, 2, 3, 4, 5, 6, 7 });

            var webContextReaderMock = new Mock<IGatewayWebContextReader>(MockBehavior.Strict);

            var loggerMock = new Mock<ILogger<EuGatewayService>>(MockBehavior.Loose);
            var keyFilterMock = new Mock<IKeyFilter>(MockBehavior.Strict);
            var storeService = new Mock<IEFGSKeyStoreService>(MockBehavior.Strict);

            var autoMapper = CreateAutoMapperWithDependencies(countryRepository);
            return CreateGatewayService(keysRepository,
                signatureServiceMock.Object,
                autoMapper,
                httpClient,
                keyFilterMock.Object,
                webContextReaderMock.Object, 
                storeService.Object,
                loggerMock.Object,
                _config
                );
        }

        private bool EqualsKeyData(ByteString protoKeyData, ByteString keyDataToCompare) => EqualsKeyData(protoKeyData.ToByteArray(), keyDataToCompare.ToByteArray());

        private bool EqualsKeyData(byte[] keyData1, byte[] keyData2) => StructuralComparisons.StructuralEqualityComparer.Equals(keyData1, keyData2);

        private IList<TemporaryExposureKeyGatewayBatchProtoDto> ParseRequestBodiesBatches(ArgumentInterceptor<HttpContent> requestArgInterceptor)
        {
            return requestArgInterceptor.Calls
                .Select(httpContentArg => httpContentArg.ReadAsByteArrayAsync().Result)
                .Select(requestBody => TemporaryExposureKeyGatewayBatchProtoDto.Parser.ParseFrom(requestBody))
                .ToList();
        }

        private EuGatewayService CreateGatewayService(
            TemporaryExposureKeyRepository keysRepository,
            ISignatureService signatureService,
            IMapper autoMapper,
            IGatewayHttpClient gateWayHttpClient,
            IKeyFilter keyFilter,
            IGatewayWebContextReader webContextReader,
            IEFGSKeyStoreService storeService,
            ILogger<EuGatewayService> logger,
            EuGatewayConfig config)
        {
            var encodingService = new EncodingService();
            var epochConverter = new EpochConverter();

            var gatewaySyncStateSettingsDao = new GatewaySyncStateSettingsDao(new SettingRepository(_dbContext));
            var settingsService = new SettingsService(gatewaySyncStateSettingsDao);

            return new EuGatewayService(
                keysRepository,
                signatureService,
                encodingService,
                keyFilter,
                webContextReader,
                autoMapper,
                logger,
                config,
                settingsService,
                epochConverter,
                gateWayHttpClient,
                storeService);
        }

        private IMapper CreateAutoMapperWithDependencies(ICountryRepository repository)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();

            services.AddSingleton(repository);
            services.AddAutoMapper(typeof(TemporaryExposureKeyToEuGatewayMapper));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMapper>();
        }
        #endregion
    }
}
