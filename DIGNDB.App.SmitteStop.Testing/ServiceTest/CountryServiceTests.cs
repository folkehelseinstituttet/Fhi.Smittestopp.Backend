using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    public class CountryServiceTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private Mock<ILogger<CountryService>> _countryLogger;

        private List<Country> _countries;

        [SetUp]
        public void Init()
        {
            _countries = new List<Country>
            {
                new Country
                {
                    Code = "SE",
                    Id = 1,
                    PullingFromGatewayEnabled = true,
                    VisitedCountriesEnabled = true,
                    EntityTranslations = new List<Translation>()
                    {
                        new Translation()
                        {
                            Value = "Sweden",
                            LanguageCountry = new Country()
                            {
                                Code = "EN"
                            }
                        },
                    }
                },
                new Country
                {
                    Code = "DE",
                    Id = 2,
                    PullingFromGatewayEnabled = true,
                    VisitedCountriesEnabled = true,
                    EntityTranslations = new List<Translation>()
                    {
                        new Translation()
                        {
                            Value = "Germany",
                            LanguageCountry = new Country()
                            {
                                Code = "EN"
                            }
                        },
                        new Translation()
                        {
                            Value = "Niemcy",
                            LanguageCountry = new Country()
                            {
                                Code = "PL"
                            }
                        }
                    }
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

            _countryLogger = new Mock<ILogger<CountryService>>();
        }

        [Test]
        public void TestGetAll()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object, _countryLogger.Object);

            var countries = countryService.GetAllCountries().Result;

            CollectionAssert.AreEquivalent(_countries, countries);
        }

        [Test]
        public void TestGetAllVisible()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object, _countryLogger.Object);

            var countries = countryService.GetVisibleCountries().Result;

            CollectionAssert.AreEquivalent(_countries, countries);
        }

        [Test]
        public void TestGetAllVisibleWithCountryCode_ShouldReturnCorrectlyOrderedList()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object, _countryLogger.Object);

            var countries = countryService.GetVisibleCountries("EN").Result.ToList();
            Assert.That(countries[0].Code == "DE");
            Assert.That(countries[0].EntityTranslations.Count == 1);
            Assert.That(countries[0].EntityTranslations.ToList()[0].Value == "Germany");
            Assert.That(countries[1].Code == "SE");
            Assert.That(countries[1].EntityTranslations.Count == 1);
            Assert.That(countries[1].EntityTranslations.ToList()[0].Value == "Sweden");
        }

        [Test]
        public void TestGetAllVisibleWithCountryCode_ShouldThrowExceptionBecauseTranslationIsMissingForOneCountry()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object, _countryLogger.Object);

            Assert.Throws<AggregateException>(() => countryService.GetVisibleCountries("PL").Result.ToList());
        }

        [Test]
        public void GetWhitelistHashSet()
        {
            var countryService = new CountryService(_countryRepositoryMock.Object, _countryLogger.Object);

            var countries = countryService.GetWhitelistHashSet().Result;

            countries.Should().BeEquivalentTo(new HashSet<long>() { 1, 2 });
        }
    }
}