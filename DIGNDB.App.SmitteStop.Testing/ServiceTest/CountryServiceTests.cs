using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Domain.Db;
using FluentAssertions;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    public class CountryServiceTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;

        private List<Country> _countries;

        [SetUp]
        public void Init()
        {
            _countries = new List<Country>
            {
                new Country
                {
                    Code = "PL",
                    Id = 1,
                    PullingFromGatewayEnabled = true,
                    VisitedCountriesEnabled = true
                },
                new Country
                {
                    Code = "SE",
                    Id = 2,
                    PullingFromGatewayEnabled = true,
                    VisitedCountriesEnabled = true
                }
            };

            _countryRepositoryMock = new Mock<ICountryRepository>();
            _countryRepositoryMock
                .Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(_countries);
            _countryRepositoryMock
                .Setup(mock => mock.GetVisibleAsync())
                .ReturnsAsync(_countries);
            _countryRepositoryMock
                .Setup(mock => mock.GetAllCountriesWithGatewayPullingEnabled())
                .ReturnsAsync(_countries);
        }

        [Test]
        public void TestGetAll()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object);

            var countries = countryService.GetAllCountries().Result;

            CollectionAssert.AreEquivalent(_countries, countries);
        }

        [Test]
        public void TestGetAllVisible()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object);

            var countries = countryService.GetVisibleCountries().Result;

            CollectionAssert.AreEquivalent(_countries, countries);
        }

        [Test]
        public void GetWhitelistHashSet()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object);

            var countries = countryService.GetWhitelistHashSet().Result;

            countries.Should().BeEquivalentTo(new HashSet<long>(){1, 2});
        }
    }
}