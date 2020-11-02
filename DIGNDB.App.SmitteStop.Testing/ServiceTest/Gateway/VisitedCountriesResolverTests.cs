using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    [TestFixture]
    public class VisitedCountriesResolverTests
    {
        [Test]
        public void TestResolve()
        {
            var visitedCountriesInDatabase = new List<Country>
            {
                 new Country {Code = "PL"},
                 new Country {Code = "DK"} 
            };

            var countryRepositoryMock = new Mock<ICountryRepository>();
            countryRepositoryMock.Setup(mock => mock.FindByIsoCodes(It.IsAny<IList<string>>()))
                .Returns(visitedCountriesInDatabase);

            var loggerMock = new Mock<ILogger<VisitedCountriesResolver>>();

            var visitedCountriesResolver = new VisitedCountriesResolver(
                countryRepositoryMock.Object,
                loggerMock.Object);

            var resultCollection =
                visitedCountriesResolver.Resolve(
                    new TemporaryExposureKeyGatewayDto()
                    {
                        VisitedCountries = new List<string>()
                        {
                            "PL",
                            "DK",
                            "EN"
                        }
                    },
                    new TemporaryExposureKey(),
                    new[] {new TemporaryExposureKeyCountry()}, null);

            Assert.IsNotNull(resultCollection);
        }
    }
}