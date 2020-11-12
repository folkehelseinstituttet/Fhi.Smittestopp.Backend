using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DIGNDB.App.SmitteStop.API;

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
            Assert.That(filteredList.Count == keyList.Count-numberOfInvalidKeys);
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

        [Test]
        public void DuplicatesAreSuccessfullyRemoved()
        {
            var keyList = _exposureKeyMock.MockListOfTemporaryExposureKeys();
            var filteredList = _keyFilter.RemoveKeyDuplicatesAsync(keyList).Result;
            Assert.That(filteredList.Count == keyList.Count-2);
        }

        [Test]
        public void RemoveKeyDuplicates_ShouldOnlyWorkOnKeysCreatedUpTo14DaysAgo()
        {
            var options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                .UseInMemoryDatabase(nameof(EuGatewayServiceUploadTest))
                .Options;

            var dbContext = new DigNDB_SmittestopContext(options);
            dbContext.Database.EnsureDeleted();
            var translationsRepositoryMock = new Mock<IGenericRepository<Translation>>(MockBehavior.Strict);

            var originSpecificSettings = new AppSettingsConfig() { OriginCuntryCode = "dk" };
            var countryRepository = new CountryRepository(dbContext, translationsRepositoryMock.Object, originSpecificSettings);
            var keysRepository = new TemporaryExposureKeyRepository(dbContext, countryRepository);

            _keyFilter = new KeyFilter(_keyMapper, _keyValidator.Object, new ExposureKeyMapper(), _logger.Object, keysRepository);

            var rollingStartNumberNewerThan14Days = DateTimeOffset.Now.Subtract(new TimeSpan(DaysOffset - 1, 0, 0, 0)).ToUnixTimeSeconds();
            var rollingStartNumberOlderThan14Days = DateTimeOffset.Now.Subtract(new TimeSpan(DaysOffset + 3, 0, 0, 0)).ToUnixTimeSeconds();
            var origin = new Country {Id = 1};
            var keysNewerThan14Days = new List<TemporaryExposureKey>
            {
                new TemporaryExposureKey {Id = Guid.NewGuid(), RollingStartNumber = rollingStartNumberNewerThan14Days, Origin = origin, KeyData = new byte[]{1}},
            };
            var keysOlderThan14Days = new List<TemporaryExposureKey>
            {
                new TemporaryExposureKey {Id = Guid.NewGuid(), RollingStartNumber = rollingStartNumberOlderThan14Days, Origin = origin, KeyData = new byte[]{2}},
                new TemporaryExposureKey {Id = Guid.NewGuid(), RollingStartNumber = rollingStartNumberOlderThan14Days, Origin = origin, KeyData = new byte[]{3}},
            };
            var keyList = keysNewerThan14Days.Concat(keysOlderThan14Days).ToList();
            dbContext.TemporaryExposureKey.AddRange(keyList);
            dbContext.SaveChanges();

            var filteredList = _keyFilter.RemoveKeyDuplicatesAsync(keyList).Result;
            filteredList.Count.Should().Be(keysOlderThan14Days.Count);
        }
    }
}
