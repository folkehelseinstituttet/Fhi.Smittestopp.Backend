using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class KeyFilterTests
    {
        public IKeyFilter _keyFilter;
        public ExposureKeyMock _exposureKeyMock { get; set; }
        public CountryMockFactory _countryFactory { get; set; }

        public IMapper _keyMapper;
        public Mock<IKeyValidator> _keyValidator;
        public Mock<ILogger<KeyFilter>> _logger;
        public Mock<ITemporaryExposureKeyRepository> _repository;
        public Mock<ICountryRepository> _countryRepository;
        public SetupMockedServices _mockServices;

        private const int DaysOffset = 14;

        [SetUp]
        public void Init()
        {
            _exposureKeyMock = new ExposureKeyMock();
            _countryFactory = new CountryMockFactory();
            _mockServices = new SetupMockedServices();
            _keyValidator = new Mock<IKeyValidator>(MockBehavior.Strict);
            _logger = new Mock<ILogger<KeyFilter>>();
            _repository = new Mock<ITemporaryExposureKeyRepository>(MockBehavior.Strict);
            _countryRepository = new Mock<ICountryRepository>(MockBehavior.Strict);
            _mockServices.SetupMapperAndCountryRepositoryMock(_countryRepository);
            _keyMapper = _mockServices.CreateAutoMapperWithDependencies(_countryRepository.Object);
            _mockServices.SetupKeyValidatorMock(_keyValidator);
            _mockServices.SetupTemopraryExposureKeyRepositoryMock(_repository);

            var mapper = new ExposureKeyMapper();
            _keyFilter = new KeyFilter(_keyMapper, _keyValidator.Object, mapper, _logger.Object, _repository.Object);
        }

        [Test]
        public void KeysAreValidatedProperly()
        {
            IList<string> errorMessageList = new List<string>();
            var keyList = _exposureKeyMock.MockListOfTemporaryExposureKeys();

            var numberOfInvalidKeys = 4;

            for (int i = 0; i < numberOfInvalidKeys; i++)
            {
                keyList.Add(_exposureKeyMock.MockInvalidKey());
            }

            var filteredList = _keyFilter.ValidateKeys(keyList, out errorMessageList);
            Assert.That(filteredList.Count == keyList.Count - numberOfInvalidKeys);
        }

        [Test]
        public void KeysAreMappedProperly()
        {
            var keyList = _exposureKeyMock.MockListOfTemporaryExposureKeyDto();
            var keyMappedList = _keyFilter.MapKeys(keyList);

            for (int i = 0; i < _exposureKeyMock.MockListLength; i++)
            {
                Assert.That(keyList[i].KeyData.SequenceEqual(keyMappedList[i].KeyData));
                Assert.That(keyList[i].RollingPeriod == Convert.ToUInt32(keyMappedList[i].RollingPeriod));
            }
        }
    }
}
