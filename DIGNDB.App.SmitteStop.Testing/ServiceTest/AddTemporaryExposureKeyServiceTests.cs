using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using DIGNDB.App.SmitteStop.Domain.Enums;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class AddTemporaryExposureKeyServiceTests
    {
        private readonly Mock<ITemporaryExposureKeyRepository> _temporaryExposureKeyRepositoryMock =
            new Mock<ITemporaryExposureKeyRepository>();
        private readonly List<TemporaryExposureKey> _exampleKeyList = new List<TemporaryExposureKey>
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

            await addTemporaryExposureKeyService.CreateKeysInDatabase(parameters, KeySource.SmitteStopApiVersion3);

            _temporaryExposureKeyRepositoryMock.Verify(mock =>
                mock.AddTemporaryExposureKeys(It.Is<IList<TemporaryExposureKey>>(keys =>
                    keys.All(key => key.KeySource == KeySource.SmitteStopApiVersion3))));

            _temporaryExposureKeyRepositoryMock.Verify(mock =>
               mock.AddTemporaryExposureKeys(It.Is<IList<TemporaryExposureKey>>(keys =>
                   keys.All(key => key.VisitedCountries.Any(country => country.Country.Code.ToLower() == "dk") == false))));
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

            var conifg = new AppSettingsConfig() { MaxKeysPerFile = 750000 };

            exposureKeyMapperMock.Setup(m =>
                    m.FilterDuplicateKeys(It.IsAny<IList<TemporaryExposureKey>>(),
                        It.IsAny<IList<TemporaryExposureKey>>()))
                .Returns(_exampleKeyList);

            var addTemporaryExposureKeyService = new AddTemporaryExposureKeyService(
                countryRepositoryMock.Object,
                temporaryExposureKeyCountryRepositoryMock.Object,
                exposureKeyMapperMock.Object,
                _temporaryExposureKeyRepositoryMock.Object, conifg);

            return addTemporaryExposureKeyService;
        }
    }
}