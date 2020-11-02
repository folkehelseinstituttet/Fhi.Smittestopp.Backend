using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using FederationGatewayApi.Config;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class GatewayDownloadServiceTests
    {

        public IEuGatewayService _euGatewayService { get; set; }
        public Mock<IGatewayHttpClient> _gatewayHttpClient;
        public Mock<ITemporaryExposureKeyRepository> _tempKeyRepository;
        public Mock<ISignatureService> _signatureService;
        public Mock<IEncodingService> _encodingService;
        public Mock<IKeyFilter> _keyFilter;
        public Mock<ICountryRepository> _countryRepository;
        public Mock<IGatewayWebContextReader> _webContextReader;
        public IMapper _mapper;
        public EuGatewayConfig _euGatewayConfig;
        public Mock<ISettingsService> _settingsService;
        public Mock<IEFGSKeyStoreService> _storeService;
        public Mock<IEpochConverter> _epochConverter { get; set; }
        public Mock<ILogger<EuGatewayService>> _logger;
        public SetupMockedServices _mockServices;

        [SetUp]
        public void Init()
        {
            _mockServices = new SetupMockedServices();
            _countryRepository = new Mock<ICountryRepository>(MockBehavior.Strict);
            _gatewayHttpClient = new Mock<IGatewayHttpClient>(MockBehavior.Strict);
            _tempKeyRepository = new Mock<ITemporaryExposureKeyRepository>(MockBehavior.Strict);
            _signatureService = new Mock<ISignatureService>(MockBehavior.Loose);
            _encodingService = new Mock<IEncodingService>(MockBehavior.Strict);
            _keyFilter = new Mock<IKeyFilter>(MockBehavior.Strict);
            _webContextReader = new Mock<IGatewayWebContextReader>(MockBehavior.Strict);
            _settingsService = new Mock<ISettingsService>(MockBehavior.Strict);
            _storeService = new Mock<IEFGSKeyStoreService>(MockBehavior.Strict);
            _tempKeyRepository = new Mock<ITemporaryExposureKeyRepository>(MockBehavior.Strict);
            _epochConverter = new Mock<IEpochConverter>(MockBehavior.Strict);

            _logger = new Mock<ILogger<EuGatewayService>>();
            _mockServices.SetupTemopraryExposureKeyRepositoryMock(_tempKeyRepository);
            _mockServices.SetupSignatureServiceMock(_signatureService);
            _mockServices.SetupEncodingServiceMock(_encodingService);
            _mockServices.SetupKeyFilterMock(_keyFilter);
            _mockServices.SetupWebContextReaderMock(_webContextReader);
            _mockServices.SetupMapperAndCountryRepositoryMock(_countryRepository);
            _mapper = _mockServices.CreateAutoMapperWithDependencies(_countryRepository.Object);
            _mockServices.SetupSettingsServiceMock(_settingsService);
            _mockServices.SetupStoreServiceMock(_storeService);
            _mockServices.SetupHttpGatewayClientMock(_gatewayHttpClient);
            _euGatewayConfig = _mockServices.CreateEuGatewayConfig();
            _mockServices.SetupEpochConverterMock(_epochConverter);

            _euGatewayService = new EuGatewayService(_tempKeyRepository.Object, _signatureService.Object, _encodingService.Object,
                                             _keyFilter.Object, _webContextReader.Object, _mapper, _logger.Object, _euGatewayConfig,
                                             _settingsService.Object, _epochConverter.Object, _gatewayHttpClient.Object,_storeService.Object);


        }
        // TODO add missing tests
    }
}
