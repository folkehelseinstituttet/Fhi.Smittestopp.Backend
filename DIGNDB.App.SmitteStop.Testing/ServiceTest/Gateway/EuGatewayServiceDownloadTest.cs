using AutoMapper;
using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Enums;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Config;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Models;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using TemporaryExposureKeyGatewayBatchProtoDto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayBatchDto;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class EuGatewayServiceDownloadTest
    {
        private DigNDB_SmittestopContext _dbContext;
        private Country _originCountry;
        private EuGatewayConfig _config;
        private Country _denmark;
        private Country _poland;
        private Country _germany;
        private Country _latvia;
        string expectedJson = "{'keyData': null,'rollingStartIntervalNumber': 0,'rollingPeriod': 0,'transmissionRiskLevel': 0,'visitedCountries': null,'origin': null,'reportType': null,'daysSinceOnsetOfSymptoms': 0}";

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

            _originCountry = _denmark;
        }

        [Test]
        public void DownloadKeys()
        {
            //Arrange
            var gatewayHttpClientMock = new Mock<IGatewayHttpClient>();
            var responseStream = GenerateStreamFromString(expectedJson);
            var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StreamContent(responseStream) };
            gatewayHttpClientMock.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                                 .ReturnsAsync(expectedResponse);

            var service = CreateGatewayServiceAndDependencies(gatewayHttpClientMock.Object);


            //Act
            service.DownloadKeysFromGateway(10);

            //Assert

            responseStream.Dispose();
            //webContextReaderMock
            //storeServiceMock
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private EuGatewayService CreateGatewayServiceAndDependencies(IGatewayHttpClient httpClient)
        {
            var translationsRepositoryMock = new Mock<IGenericRepository<Translation>>(MockBehavior.Strict);
            var gatewayDto = new TemporaryExposureKeyGatewayDto();

            IOriginSpecificSettings originConfig = new AppSettingsConfig() { OriginCountryCode = _originCountry.Code.ToUpper() };
            var countryRepository = new CountryRepository(_dbContext, translationsRepositoryMock.Object, originConfig);
            var keysRepository = new TemporaryExposureKeyRepository(_dbContext, countryRepository);

            var signatureServiceMock = new Mock<ISignatureService>(MockBehavior.Strict);
            signatureServiceMock.Setup(sigService => sigService.Sign(It.IsAny<TemporaryExposureKeyGatewayBatchProtoDto>(), Domain.SortOrder.ASC))
                .Returns(new byte[] { 1, 2, 3, 4, 5, 6, 7 });

            var webContextReaderMock = new Mock<IGatewayWebContextReader>(MockBehavior.Strict);
            webContextReaderMock.Setup(mock => mock.ReadHttpContextStream(It.IsAny<HttpResponseMessage>())).Returns(expectedJson);
            webContextReaderMock.Setup(mock => mock.GetItemsFromRequest(It.IsAny<string>())).Returns(new List<TemporaryExposureKeyGatewayDto> { gatewayDto });

            var loggerMock = new Mock<ILogger<EuGatewayService>>(MockBehavior.Loose);
            var keyFilterMock = new Mock<IKeyFilter>(MockBehavior.Strict);
            var storeService = new Mock<IEFGSKeyStoreService>(MockBehavior.Strict);
            storeService.Setup(mock => mock.FilterAndSaveKeys(It.IsAny<IList<TemporaryExposureKeyGatewayDto>>())).Returns(new List<TemporaryExposureKey> { new TemporaryExposureKey() });

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
    }
}
