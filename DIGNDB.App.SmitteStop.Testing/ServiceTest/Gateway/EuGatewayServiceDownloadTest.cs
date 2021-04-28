using AutoMapper;
using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using DIGNDB.App.SmitteStop.Testing.UtilTest;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Config;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Models;
using FederationGatewayApi.Services;
using FederationGatewayApi.Services.Settings;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class EuGatewayServiceDownloadTest
    {
        private const string BatchTagHeaderName = "BatchTag";
        private const string DateFormat = "yyyy-MM-dd";


        private DigNDB_SmittestopContext _dbContext;
        private CountryRepository _countryRepository;
        private IMapper _autoMapper;
        private EuGatewayConfig _config;
        private IEpochConverter _epochConverter;
        private Mock<ILogger<TemporaryExposureKeyRepository>> _temporaryExposureKeyRepositoryLogger;

        private Country _denmark;
        private Country _poland;
        private Country _germany;
        private Country _latviaDisabledDownload;
        private readonly Country _spain;
        private TemporaryExposureKeyRepository _keysRepository;

        [SetUp]
        public void CreateDataSet()
        {
            var options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                                .UseInMemoryDatabase(nameof(EuGatewayServiceUploadTest))
                                .Options;

            _dbContext = new DigNDB_SmittestopContext(options);
            _dbContext.Database.EnsureDeleted();

            _temporaryExposureKeyRepositoryLogger = new Mock<ILogger<TemporaryExposureKeyRepository>>(MockBehavior.Loose);
            var translationsRepositoryMock = new Mock<IGenericRepository<Translation>>(MockBehavior.Strict);
            _countryRepository = new CountryRepository(_dbContext, translationsRepositoryMock.Object, new AppSettingsConfig());
            _autoMapper = CreateAutoMapperWithDependencies(_countryRepository);

            _config = new EuGatewayConfig()
            {
                AuthenticationCertificateFingerprint = "AuthenticationCertificateFingerprint",
                SigningCertificateFingerprint = "SigningCertificateFingerprint",
                Url = "http://netcompany.pl",
            };

            _epochConverter = new EpochConverter();

            _denmark = TestCountryBuilder.Denmark.Build();
            _poland = TestCountryBuilder.Poland.Build();
            _germany = TestCountryBuilder.Germany.Build();
            _latviaDisabledDownload = TestCountryBuilder.Latvia
                .SetIsPullingFromGatewayEnabled(false)
                .Build();

            _dbContext.AddRange(_denmark, _poland, _germany, _latviaDisabledDownload);
            _dbContext.SaveChanges();
        }
        
        [TestCase]
        public void DownloadKeysFromMultipleCountries_ShouldSaveOnlyValidKeysFromEnabledOrigins()
        {
            int maximumNumberOfDaysBack = 3;

            var today = DateTime.UtcNow.Date;
            var day1DateStr = today.AddDays(-2).ToString(DateFormat);
            var day2DateStr = today.AddDays(-1).ToString(DateFormat);
            var day3DateStr = today.ToString(DateFormat);

            var germanDefaultBuilder = TestTemporaryExposureKeyBuilder.CreateDefault(_germany)
                .SetReportType(ReportType.CONFIRMED_TEST)
                .SetKeySource(KeySource.Gateway);

            // has DK in VisitedCountries - should be accepted
            var germanValidKeysBuilder1 = germanDefaultBuilder.Copy().SetVisitedCountries(new[] { _poland, _denmark });
            // has empty VisitedCountries = no filtering. Should be accepted
            var germanValidKeysBuilder2 = germanDefaultBuilder.Copy().SetVisitedCountries(new Country[] { });
            // doesn't have DK in VisitedCountries - should be accepted because of one world policy
            var germanValidKeysBuilder3 = germanDefaultBuilder.Copy().SetVisitedCountries(new Country[] { _poland });
            // invalid RollingStartNumber - should be rejected
            var germanInvalidKeysBuilder1 = germanDefaultBuilder.Copy()
                .SetVisitedCountries(new Country[] { _denmark })
                .SetRollingStartNumber(DateTime.UnixEpoch);
            // pulling from Latvia is disabled - should be rejected
            var latviaKeysBuilder = TestTemporaryExposureKeyBuilder.CreateDefault(_latviaDisabledDownload)
                .SetKeySource(KeySource.Gateway);

            // BE AWARE THAT KeyData CANNOT BE LONGER THAN 16 BYTES - longer keys will be rejected by GatewayService
            // day 1 - batch 1
            var daysOneBatchTag1 = day1DateStr;
            var dayOneValidKeysBatch1 = new List<TemporaryExposureKey>()
                .Concat(germanValidKeysBuilder1.BuildNormalized(new[] { "DE1.1_V1", "DE1_V2", "DE1.1_V3" }))
                .Concat(germanValidKeysBuilder2.BuildNormalized(new[] { "DE1.1_V4", "DE1.1_V5", "DE1.1_V6" }));
            var dayOneValidKeysBatch4 = new List<TemporaryExposureKey>()
                .Concat(germanValidKeysBuilder3.BuildNormalized(new[] { "DE1.1_INV1", "DE1.1_INV2" }));
            // day #1 - batch 2
            var daysOneBatchTag2 = day1DateStr + "-2";
            var dayOneValidKeysBatch2 = new List<TemporaryExposureKey>()
                .Concat(germanValidKeysBuilder1.BuildNormalized(new[] { "DE1.2_V1", "DE1.2_V2", "DE1.2_V3" }));
            var dayOneInvalidKeysBatch2 = new List<TemporaryExposureKey>()
                 .Concat(latviaKeysBuilder.BuildNormalized(new[] { "LT1.2_INV1", "LT1.2_INV2" }));
            // day #1 - batch 3
            var daysOneBatchTag3 = day1DateStr + "-3";
            var dayOneValidKeysBatch3 = new List<TemporaryExposureKey>()
                .Concat(germanValidKeysBuilder1.BuildNormalized(new[] { "DE1.3_V1", "DE1.3_V2", "DE1.3_V3" }));
            // day #2 - batch 1
            var dayTwoBatchTag1 = day2DateStr;
            var dayTwoInvalidKeysBatch1 = new List<TemporaryExposureKey>()
                .Concat(latviaKeysBuilder.BuildNormalized(new[] { "LT2.1_INV1", "LT2.1_INV2" }));
            // day #2 - batch 2
            var dayTwoBatchTag2 = day2DateStr + "-2";
            var dayTwoInvalidKeysBatch2 = new List<TemporaryExposureKey>()
                .Concat(latviaKeysBuilder.BuildNormalized(new[] { "LT2.2_INV1", "LT2.2_INV2" }))
                .Concat(germanInvalidKeysBuilder1.BuildNormalized(new[] { "DE2.2_INV3" }));
            // day #3 - batch 1
            var dayThreeBatchTag1 = day3DateStr;
            var dayThreeValidKeysBatch1 = new List<TemporaryExposureKey>()
                .Concat(germanValidKeysBuilder1.BuildNormalized(new[] { "DE3.1_V1", "DE3_V2", "DE3.1_V3" }))
                .Concat(germanValidKeysBuilder2.BuildNormalized(new[] { "DE3.1_V4", "DE3.1_V5", "DE3.1_V6" }));

            var keyBatchesDict = new Dictionary<string, IEnumerable<TemporaryExposureKey>>();
            keyBatchesDict.Add(key: daysOneBatchTag1, value: dayOneValidKeysBatch1.Concat(dayOneValidKeysBatch4));
            keyBatchesDict.Add(key: daysOneBatchTag2, value: dayOneValidKeysBatch2.Concat(dayOneInvalidKeysBatch2));
            keyBatchesDict.Add(key: daysOneBatchTag3, value: dayOneValidKeysBatch3);

            keyBatchesDict.Add(key: dayTwoBatchTag1, value: dayTwoInvalidKeysBatch1);
            keyBatchesDict.Add(key: dayTwoBatchTag2, value: dayTwoInvalidKeysBatch2);

            keyBatchesDict.Add(key: dayThreeBatchTag1, value: dayThreeValidKeysBatch1);

            // setup mock
            var gatewayHttpClientMock = new Mock<IGatewayHttpClient>(MockBehavior.Strict);
            gatewayHttpClientMock
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((HttpRequestMessage request) =>
                {
                    string dictKey;
                    // request for the first batch doesn't contain BatchTag header
                    if (!request.Headers.Contains(BatchTagHeaderName))
                        dictKey = ExtractDateFromUrl(request.RequestUri.ToString());
                    else
                        dictKey = request.Headers.GetValues(BatchTagHeaderName).First();

                    if (keyBatchesDict.ContainsKey(dictKey))
                    {
                        return CreateGatewayResponseFromBatch(keyBatchesDict[dictKey]);
                    }
                    else throw new InvalidOperationException($"Invalid batchTag has been requested: {dictKey}");
                });
            var service = CreateGatewayServiceAndDependencies(gatewayHttpClientMock.Object);

            // Act
            service.DownloadKeysFromGateway(maximumNumberOfDaysBack);

            // .: Verify
            // get data
            var allSavedKeys = _keysRepository.GetAll().Result;

            // assert
            var allValidKeys = new List<TemporaryExposureKey>()
                .Concat(dayOneValidKeysBatch1)
                .Concat(dayOneValidKeysBatch2)
                .Concat(dayOneValidKeysBatch3)
                .Concat(dayOneValidKeysBatch4)
                .Concat(dayThreeValidKeysBatch1)
                .ToList();

            var allInvalidKeys = new List<TemporaryExposureKey>()
                .Concat(dayOneInvalidKeysBatch2)
                .Concat(dayOneInvalidKeysBatch2)
                .Concat(dayTwoInvalidKeysBatch1)
                .Concat(dayTwoInvalidKeysBatch2)
                .ToList();

            allSavedKeys.Should()
                .OnlyHaveUniqueItems(key => key.KeyData, because: "Service cannot send duplicates.")
                .And.OnlyContain(key2Check => IsInCollection(key2Check.KeyData, allValidKeys), because: "Only valid keys should be save in the database.");
        }

        #region Helpers

        private bool IsInCollection(byte[] keyData, IEnumerable<TemporaryExposureKey> collection2Check)
        {
            return collection2Check.Any(keyFromCollection => KeyDataComperator.EqualsKeyData(keyData, keyFromCollection.KeyData));
        }

        private string ExtractDateFromUrl(string downloadUrl)
        {
            var validUrl = _config.UrlNormalized + "diagnosiskeys/download/";
            int startIndex = downloadUrl.IndexOf(validUrl) + validUrl.Length;
            return downloadUrl.Substring(startIndex);
        }

        private HttpResponseMessage CreateGatewayResponseFromBatch(IEnumerable<TemporaryExposureKey> keyPackage)
        {
            var keys = keyPackage.Select(entityKey => _autoMapper.Map<TemporaryExposureKeyGatewayDto>(entityKey)).ToList();
            var gatewayProtoKeys = keys.Select(key =>
                _autoMapper.Map<FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayDto>(key));
            var gatewayBatchProto = new FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayBatchDto() { Keys = { gatewayProtoKeys } };
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(gatewayBatchProto.ToString()) };
        }

        private EuGatewayService CreateGatewayServiceAndDependencies(IGatewayHttpClient httpClient)
        {
            _keysRepository = new TemporaryExposureKeyRepository(_dbContext, _countryRepository, _temporaryExposureKeyRepositoryLogger.Object);

            var signatureServiceMock = new Mock<ISignatureService>(MockBehavior.Strict);

            var webContextReader = new GatewayWebContextReader(_autoMapper);

            var epochConverter = new EpochConverter();

            var keyValidator = new KeyValidator(epochConverter);
            var exposureKeyMapper = new ExposureKeyMapper(_epochConverter);
            var keyFilterLoggerMock = new Mock<ILogger<KeyFilter>>(MockBehavior.Loose);
            var keyFilter = new KeyFilter(_autoMapper, keyValidator, exposureKeyMapper, keyFilterLoggerMock.Object, _keysRepository);

            var storeServiceLoggerMock = new Mock<ILogger<EFGSKeyStoreService>>();

            var riskCalculator = new RiskCalculator();
            var dsosDecoder = new DaysSinceOnsetOfSymptomsDecoder();
            var addTemporaryExposureKeyService = new Mock<IAddTemporaryExposureKeyService>(MockBehavior.Strict);
            addTemporaryExposureKeyService
                .Setup(x => x.FilterDuplicateKeysAsync(It.IsAny<List<TemporaryExposureKey>>()))
                .ReturnsAsync((List<TemporaryExposureKey> argument) =>
                {
                    return argument;
                });
            var storeService = new EFGSKeyStoreService(keyFilter, _keysRepository, storeServiceLoggerMock.Object, riskCalculator, epochConverter, dsosDecoder, addTemporaryExposureKeyService.Object);
            var gatewayServiceLoggerMock = new Mock<ILogger<EuGatewayService>>(MockBehavior.Loose);

            return CreateGatewayService(
                _keysRepository,
                signatureServiceMock.Object,
                _autoMapper,
                httpClient,
                keyFilter,
                webContextReader,
                storeService,
                gatewayServiceLoggerMock.Object,
                epochConverter,
                _config
                );
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
           IEpochConverter epochConverter,
           EuGatewayConfig config)
        {
            var encodingService = new EncodingService();

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
                storeService
                );
        }

        private IMapper CreateAutoMapperWithDependencies(ICountryRepository repository)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();

            services.AddSingleton(repository);
            services.AddAutoMapper(typeof(TemporaryExposureKeyToEuGatewayMapper));
            services.AddAutoMapper(typeof(EuGatewayProtoToDtosMapper));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMapper>();
        }

        #endregion
    }
}
