using System.Collections.Generic;
using System.Linq;
using Bogus;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class CountryRepositoryTests
    {
        private const string ExampleCountryCodePl = "PL";
        private const string ExampleCountryCodeDk = "DK";

        [Test]
        public void TestGetAllAsync()
        {
            var fakeCountries = GenerateFakeCountries();
            var countryRepository = CreateCountryRepository(fakeCountries);

            var resultCollection = countryRepository.GetAllAsync().Result;
            CollectionAssert.AreEquivalent(fakeCountries, resultCollection);
        }

        [Test]
        public void TestFindByIsoCode()
        {
            var fakeCountries = GenerateFakeCountries();
            var countryRepository = CreateCountryRepository(fakeCountries);

            var resultCountry = countryRepository.FindByIsoCode(ExampleCountryCodePl);

            Assert.AreSame(resultCountry, fakeCountries.FirstOrDefault());
        }

        [Test]
        public void TestFindByIsoCodes()
        {
            var fakeCountries = GenerateFakeCountries();
            var countryRepository = CreateCountryRepository(fakeCountries);

            var resultCountries =
                countryRepository.FindByIsoCodes(new List<string> {ExampleCountryCodePl, ExampleCountryCodeDk});

            var resultArray = resultCountries.ToArray();
            resultArray[0].Id.Should().Be(fakeCountries[0].Id);
            resultArray[1].Id.Should().Be(fakeCountries[1].Id);
        }

        [Test]
        public void TestGetDenmarkCountry()
        {
            var fakeCountries = GenerateFakeCountries();
            var countryRepository = CreateCountryRepository(fakeCountries);

            var denmarkCountry = countryRepository.GetDenmarkCountry();

            denmarkCountry.Code.Should().Be(ExampleCountryCodeDk);
        }

        private CountryRepository CreateCountryRepository(Country[] fakeCountries)
        {
            DbContextOptions<DigNDB_SmittestopContext> options =
                new DbContextOptionsBuilder<DigNDB_SmittestopContext>()
                    .UseInMemoryDatabase(nameof(CountryRepositoryTests)).Options;
            var context = new DigNDB_SmittestopContext(options);
            context.Database.EnsureDeleted();

            fakeCountries[0].Code = ExampleCountryCodePl;
            fakeCountries[1].Code = ExampleCountryCodeDk;

            var exposureKeyCountries = new List<TemporaryExposureKeyCountry>
            {
                new TemporaryExposureKeyCountry
                {
                    CountryId = 1
                },
                new TemporaryExposureKeyCountry
                {
                    CountryId = 2
                },
            };

            context.AddRange(fakeCountries);
            context.AddRange(exposureKeyCountries);
            context.SaveChanges();

            var translationRepositoryMock = new Mock<IGenericRepository<Translation>>();

            return new CountryRepository(context, translationRepositoryMock.Object);
        }

        private Country[] GenerateFakeCountries()
        {
            var fakeCountries = new Faker<Country>();

            return fakeCountries.Generate(3).ToArray();
        }
    }
}