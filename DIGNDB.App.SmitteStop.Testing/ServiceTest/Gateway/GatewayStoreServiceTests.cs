using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using FederationGatewayApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class GatewayStoreServiceTests
    {

        private IEFGSKeyStoreService _keyStoreService;

        private Mock<IGatewayWebContextReader> _webContextReader;

        private Mock<IKeyFilter> _keyFilter;

        private Mock<ITemporaryExposureKeyRepository> _tempKeyRepository;

        private Mock<ILogger<EFGSKeyStoreService>> _logger;
        private EpochConverter _epochConverter;
        private DaysSinceOnsetOfSymptomsDecoder _onsetDaysDecoder;
        private RiskCalculator _riskCalulator;

        private ExposureKeyMock _exposureKeyMock;

        private WebContextMock _webContextMock;

        private SetupMockedServices _mockSetup;

        [SetUp]
        public void Init()
        {
            _riskCalulator = new RiskCalculator();
            _exposureKeyMock = new ExposureKeyMock();
            _webContextMock = new WebContextMock();
            _mockSetup = new SetupMockedServices();
            _webContextReader = new Mock<IGatewayWebContextReader>(MockBehavior.Strict);
            _keyFilter = new Mock<IKeyFilter>(MockBehavior.Strict);
            _tempKeyRepository = new Mock<ITemporaryExposureKeyRepository>(MockBehavior.Strict);
            _logger = new Mock<ILogger<EFGSKeyStoreService>>();
            _epochConverter = new EpochConverter();
            _onsetDaysDecoder = new DaysSinceOnsetOfSymptomsDecoder();

            _mockSetup.SetupWebContextReaderMock(_webContextReader);
            _mockSetup.SetupKeyFilterMock(_keyFilter);
            _mockSetup.SetupTemopraryExposureKeyRepositoryMock(_tempKeyRepository);

            _keyStoreService = new EFGSKeyStoreService(_webContextReader.Object, _keyFilter.Object, _tempKeyRepository.Object, _logger.Object, _riskCalulator, _epochConverter, _onsetDaysDecoder);

        }

        [Test]
        public void KeysShouldBeStoredProperly()
        {
            IList<string> errorMessageList = new List<string>();
            IList<TemporaryExposureKeyGatewayDto> keys = _exposureKeyMock.MockListOfTemporaryExposureKeyDto();
            var filteredKeys = _keyStoreService.FilterAndSaveKeys(keys);
            // TODO this is only verify if some method has been called. It is not verify if keys are properly filtered and saved
            /*
            _webContextReader.Verify(mock => mock.GetItemsFromRequest(It.IsAny<string>()));
            _keyFilter.Verify(mock => mock.MapKeys(It.IsAny<IList<TemporaryExposureKeyGatewayDto>>()));
            _keyFilter.Verify(mock => mock.ValidateKeys(It.IsAny<IList<TemporaryExposureKey>>(), out errorMessageList));
            _keyFilter.Verify(mock => mock.RemoveKeyDuplicates(It.IsAny<IList<TemporaryExposureKey>>()));
            _tempKeyRepository.Verify(mock => mock.AddTemporaryExposureKeys(It.IsAny<IList<TemporaryExposureKey>>()));*/
        }

        [Test]
        public void KeysAreNotStoredProperlyWhenTheWebContextIsInvalid()
        {
            IList<string> errorMessageList = new List<string>();
            IList<TemporaryExposureKeyGatewayDto> keys = _exposureKeyMock.MockListOfTemporaryExposureKeyDto();

            _mockSetup.SetupWebContextReaderMockWithBadContext(_webContextReader);

            _webContextReader.Verify(mock => mock.GetItemsFromRequest(It.IsAny<string>()), Times.Never);
            _keyFilter.Verify(mock => mock.MapKeys(It.IsAny<IList<TemporaryExposureKeyGatewayDto>>()), Times.Never);
            _keyFilter.Verify(mock => mock.ValidateKeys(It.IsAny<IList<TemporaryExposureKey>>(), out errorMessageList), Times.Never);
            _keyFilter.Verify(mock => mock.RemoveKeyDuplicatesAsync(It.IsAny<List<TemporaryExposureKey>>()), Times.Never);
            _tempKeyRepository.Verify(mock => mock.AddTemporaryExposureKeys(It.IsAny<List<TemporaryExposureKey>>()), Times.Never);

        }

    }
}
