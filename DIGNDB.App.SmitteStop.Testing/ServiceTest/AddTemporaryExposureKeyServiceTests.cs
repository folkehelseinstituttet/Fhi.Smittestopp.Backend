using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class AddTemporaryExposureKeyServiceTests
    {
        private readonly Mock<ITemporaryExposureKeyRepository> _temporaryExposureKeyRepositoryMock =
            new Mock<ITemporaryExposureKeyRepository>();

        readonly List<TemporaryExposureKey> _exampleKeyList = new List<TemporaryExposureKey>
        {
            new TemporaryExposureKey(),
            new TemporaryExposureKey(),
            new TemporaryExposureKey()
        };

        [Test]
        public async Task TestCreateKeysInDatabase()
        {
            var addTemporaryExposureKeyService = CreateTestObject();
            var parameters = new TemporaryExposureKeyBatchDto
            {
                appPackageName = string.Empty,
                visitedCountries = new List<string>
                {
                    "CR",
                    "PL",
                    "DK"
                },
                regions = new List<string>
                {
                    "dk"
                },
            };

            await addTemporaryExposureKeyService.CreateKeysInDatabase(parameters);

            _temporaryExposureKeyRepositoryMock.Verify(mock =>
                mock.AddTemporaryExposureKeys(It.Is<IList<TemporaryExposureKey>>(keys =>
                    keys.All(key => key.KeySource == KeySource.SmitteStopApiVersion2))));   

            _temporaryExposureKeyRepositoryMock.Verify(mock =>
               mock.AddTemporaryExposureKeys(It.Is<IList<TemporaryExposureKey>>(keys =>
                   keys.All(key => key.VisitedCountries.Any( country =>country.Country.Code.ToLower() == "dk" ) == false))));
        }

        public AddTemporaryExposureKeyService CreateTestObject()
        {
            _temporaryExposureKeyRepositoryMock.Setup(x => x.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<TemporaryExposureKey>());
            _temporaryExposureKeyRepositoryMock.Setup(x => x.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(It.IsAny<long>(), 0, It.IsAny<int>())).ReturnsAsync(new List<TemporaryExposureKey>() { new TemporaryExposureKey() });
            var countryRepositoryMock = new Mock<ICountryRepository>();
            countryRepositoryMock.Setup(
                    m => m.FindByIsoCode(It.IsAny<string>()))
                .Returns(new Country()
                {
                    Code = "DK"
                });

            var temporaryExposureKeyCountryRepositoryMock = new Mock<IGenericRepository<TemporaryExposureKeyCountry>>();
            var exposureKeyMapperMock = new Mock<IExposureKeyMapper>();
            exposureKeyMapperMock.Setup(x => x.FromDtoToEntity(It.IsAny<TemporaryExposureKeyBatchDto>())).Returns(new List<TemporaryExposureKey>());
            var configuration = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("false");
            configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
            var appSettingsMock = new Mock<IAppSettingsConfig>();
            appSettingsMock.Setup(mock => mock.Configuration).Returns(configuration.Object);
            exposureKeyMapperMock.Setup(m =>
                    m.FilterDuplicateKeys(It.IsAny<IList<TemporaryExposureKey>>(),
                        It.IsAny<IList<TemporaryExposureKey>>()))
                .Returns(_exampleKeyList);

            var addTemporaryExposureKeyService = new AddTemporaryExposureKeyService(
                countryRepositoryMock.Object,
                temporaryExposureKeyCountryRepositoryMock.Object,
                exposureKeyMapperMock.Object,
                _temporaryExposureKeyRepositoryMock.Object, appSettingsMock.Object);

            return addTemporaryExposureKeyService;
        }
    }
}