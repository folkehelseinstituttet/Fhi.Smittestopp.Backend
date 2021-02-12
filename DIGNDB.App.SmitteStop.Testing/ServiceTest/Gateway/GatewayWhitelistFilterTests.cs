using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    [TestFixture]
    public class GatewayWhitelistFilterTests
    {
        private readonly Guid _exampleGuid1 = Guid.NewGuid();
        private readonly Guid _exampleGuid2 = Guid.NewGuid();
        private readonly Guid _exampleGuid3 = Guid.NewGuid();
        private readonly Guid _exampleGuid4 = Guid.NewGuid();
        private readonly Guid _exampleGuid5 = Guid.NewGuid();

        [Test]
        public void TestFilterKeys()
        {
            IGatewayWhitelistFilter filter = CreateTestObject();

            var keysToBeFiltered = CreateKeysToBeFiltered();

            filter.FilterKeys(keysToBeFiltered);

            var expectedKeysAfterFiltering = CreateFilteredKeys();

            keysToBeFiltered.Should().BeEquivalentTo(expectedKeysAfterFiltering);
        }

        private Task<HashSet<long>> CreateWhitelistOfCountries()
        {
            return Task.FromResult(new HashSet<long>(new List<long> {1, 2, 4, 5}));
        }

        private List<TemporaryExposureKey> CreateKeysToBeFiltered()
        {
            return new List<TemporaryExposureKey>
            {
                new TemporaryExposureKey { Id = _exampleGuid1, Origin = new Country { Id = 1 }},
                new TemporaryExposureKey { Id = _exampleGuid2, Origin = new Country { Id = 2 }},
                new TemporaryExposureKey { Id = _exampleGuid3, Origin = new Country { Id = 3 }},
                new TemporaryExposureKey { Id = _exampleGuid4, Origin = new Country { Id = 5 }},
                new TemporaryExposureKey { Id = _exampleGuid5, Origin = new Country { Id = 6 }},
            };
        }

        private List<TemporaryExposureKey> CreateFilteredKeys()
        {
            return new List<TemporaryExposureKey>
            {
                new TemporaryExposureKey { Id = _exampleGuid1, Origin = new Country { Id = 1 }},
                new TemporaryExposureKey { Id = _exampleGuid2, Origin = new Country { Id = 2 }},
                new TemporaryExposureKey { Id = _exampleGuid4, Origin = new Country { Id = 5 }},
            };
        }

        private IGatewayWhitelistFilter CreateTestObject()
        {
            var loggerMock = new Mock<ILogger<GatewayWhitelistFilter>>();
            var countryServiceMock = new Mock<ICountryService>();
            countryServiceMock.Setup(mock => mock.GetWhitelistHashSet())
                .Returns(CreateWhitelistOfCountries());

            IGatewayWhitelistFilter filter = new GatewayWhitelistFilter(loggerMock.Object, countryServiceMock.Object);

            return filter;
        }
    }
}