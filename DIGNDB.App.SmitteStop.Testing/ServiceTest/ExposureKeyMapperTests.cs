using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using FederationGatewayApi.Mappers;
using FederationGatewayApi.Models;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExposureKeyMapperTests
    {
        private const string NonExistingCountryCode = "XY";

        private IList<TemporaryExposureKey> CreateMockedListExposureKeys(int listCount)
        {
            List<TemporaryExposureKey> keys = new List<TemporaryExposureKey>();
            for (var i = 1; i <= listCount; i++)
            {
                keys.Add(new TemporaryExposureKey()
                {
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyToday" + i),
                    RollingPeriod = 144,
                    RollingStartNumber = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMinutes / 10,
                    CreatedOn = DateTime.UtcNow,
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
                });
            }
            return keys;
        }

        [Test]
        public void FromEntityToProto_GiveEntity_ShouldReturnCorrectProtoModel()
        {
            var mockEntity = CreateMockedExposureKey();

            var mapper = new ExposureKeyMapper();
            var protoModel = mapper.FromEntityToProto(mockEntity);

            Assert.AreEqual(mockEntity.KeyData, protoModel.KeyData);
            Assert.AreEqual((int)mockEntity.RollingPeriod, protoModel.RollingPeriod);
            Assert.AreEqual((int)mockEntity.RollingStartNumber / (60 * 10), protoModel.RollingStartIntervalNumber);
            Assert.AreEqual((int)mockEntity.TransmissionRiskLevel, protoModel.TransmissionRiskLevel);
            Assert.IsInstanceOf<Domain.Proto.TemporaryExposureKey>(protoModel);
        }

        [Test]
        public void FromEntityToProtoBatch_GiveEntityList_ShouldReturnCorrectProtoBatchModel()
        {
            int listCount = 3;
            var keys = CreateMockedListExposureKeys(listCount);
            var keysByTime = keys.OrderBy(k => k.CreatedOn);
            var startTimes = new DateTimeOffset(keysByTime.First().CreatedOn);
            var endTimes = new DateTimeOffset(keysByTime.Last().CreatedOn);

            var mapper = new ExposureKeyMapper();
            var protoBatch = mapper.FromEntityToProtoBatch(keys);

            Assert.AreEqual(listCount, protoBatch.Keys.Count);
            Assert.AreEqual(protoBatch.BatchNum, 1);
            Assert.AreEqual(protoBatch.BatchSize, 1);
            Assert.AreEqual(protoBatch.StartTimestamp, startTimes.ToUnixTimeSeconds());
            Assert.AreEqual(protoBatch.EndTimestamp, endTimes.ToUnixTimeSeconds());
            Assert.AreEqual(protoBatch.Region, "DK");
            Assert.IsInstanceOf<Domain.Proto.TemporaryExposureKeyExport>(protoBatch);
        }

        [Test]
        public void MapTemporaryExposureKey_ToTemporaryExposureKeyGatewayDto_ShouldGetCorrectValue()
        {
            // .: Arrange
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);
            var mapper = CreateAutoMapperWithDependencies(countryRepositoryMock.Object);
            var sourceKey = CreateMockedExposureKey();

            // .: Act
            var destinationEfgsKey = mapper.Map<TemporaryExposureKey, TemporaryExposureKeyGatewayDto>(sourceKey);

            // .: Assert
            destinationEfgsKey.KeyData.Should()
                .NotBeNull()
                .And.Equal(sourceKey.KeyData);
            destinationEfgsKey.RollingPeriod.Should()
                .IsSameOrEqualTo(sourceKey.RollingPeriod);
            destinationEfgsKey.RollingStartIntervalNumber.Should()
                .IsSameOrEqualTo(sourceKey.RollingStartNumber);
            destinationEfgsKey.ReportType.Should()
                .IsSameOrEqualTo(Enum.GetName(typeof(ReportType), sourceKey.ReportType));
            destinationEfgsKey.Origin.Should().NotBeNull()
                .IsSameOrEqualTo(sourceKey.Origin.Code);
            destinationEfgsKey.TransmissionRiskLevel.Should()
                .IsSameOrEqualTo(sourceKey.TransmissionRiskLevel);
            destinationEfgsKey.VisitedCountries.Should()
                .BeInAscendingOrder()
                .And.IsSameOrEqualTo(sourceKey.VisitedCountries.Select(c => c.Country.Code));
            destinationEfgsKey.DaysSinceOnsetOfSymptoms.Should()
                .Be(sourceKey.DaysSinceOnsetOfSymptoms);
            destinationEfgsKey.ReportType.Should().Equals(ReportType.CONFIRMED_CLINICAL_DIAGNOSIS);
        }

        [Test]
        public void MapTemporaryExposureKeyGatewayDto_ToTemporaryExposureKey_ShouldGetCorrectValue()
        {
            (var mapper, var sourceKey, var country) = CreateMapperSourceKeyAndCountry();

            // .: Act
            var destinationKey = mapper.Map<TemporaryExposureKeyGatewayDto, TemporaryExposureKey>(sourceKey);

            // .: Assert
            destinationKey.KeyData.Should()
                .NotBeNull()
                .And.IsSameOrEqualTo(sourceKey.KeyData);
            destinationKey.RollingPeriod.Should()
                .IsSameOrEqualTo(sourceKey.RollingPeriod);
            destinationKey.RollingStartNumber.Should()
                .IsSameOrEqualTo(sourceKey.RollingStartIntervalNumber);
            destinationKey.ReportType.Should()
                .IsSameOrEqualTo(Enum.Parse(typeof(ReportType), sourceKey.ReportType));
            destinationKey.Origin.Should()
                .NotBeNull()
                .And.IsSameOrEqualTo(country);
            destinationKey.TransmissionRiskLevel.Should()
                .IsSameOrEqualTo(RiskLevel.RISK_LEVEL_LOW);
            destinationKey.VisitedCountries
                .Select(s => s.Country.Code).Should()
                .NotBeNull()
                .And.Contain(sourceKey.VisitedCountries);
            destinationKey.ReportType.Should().Equals(sourceKey.ReportType);
        }

        private const int MinInvalidRange = 2986;
        private const int MaxInvalidRange = 3014;

        private const int MinValidRange = -14;
        private const int MaxValidRange = 14;

        private static readonly IEnumerable<int> InvalidRange =
            Enumerable.Range(MinInvalidRange, MaxInvalidRange - MinInvalidRange + 1);
        private static readonly IEnumerable<int> ValidRange =
            Enumerable.Range(MinValidRange, MaxValidRange - MinValidRange + 1);

        private static readonly IEnumerable<int> BothRanges = InvalidRange.Concat(ValidRange);

        [Test]
        public void MapTemporaryExposureKeyGatewayDto_ToTemporaryExposureKey_ShouldTranslateDaysSinceOnsetOfSymptomsToTheSameValueIfInSetRanges(
            [ValueSource(nameof(BothRanges))] int daysSinceOnsetOfSymptoms)
        {
            (var mapper, var sourceKey, var country) = CreateMapperSourceKeyAndCountry();
            sourceKey.DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;

            // .: Act
            var destinationKey = mapper.Map<TemporaryExposureKeyGatewayDto, TemporaryExposureKey>(sourceKey);

            // .: Assert
            destinationKey.DaysSinceOnsetOfSymptoms
                .Should().Be(daysSinceOnsetOfSymptoms);
        }

        [TestCase(MinValidRange - 1)]
        [TestCase(MaxValidRange + 1)]
        [TestCase(MinInvalidRange - 1)]
        [TestCase(MaxInvalidRange + 1)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public void MapTemporaryExposureKeyGatewayDto_ToTemporaryExposureKey_ShouldTranslateDaysSinceOnsetOfSymptomsToTheSameValueIfInNotSetRanges(
            int daysSinceOnsetOfSymptoms)
        {
            (var mapper, var sourceKey, var country) = CreateMapperSourceKeyAndCountry();
            sourceKey.DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;

            // .: Act
            var destinationKey = mapper.Map<TemporaryExposureKeyGatewayDto, TemporaryExposureKey>(sourceKey);

            // .: Assert
            destinationKey.DaysSinceOnsetOfSymptoms
                .Should().Be(null);
        }

        [Test]
        public void MapTemporaryExposureKeyGatewayDto_ToTemporaryExposureKey_ShouldThrowExceptionIfCountryNotExisting()
        {
            var tuple = CreateMapperSourceKeyAndCountry();
            tuple.sourceKey.Origin = NonExistingCountryCode;

            Action mappingAction = () =>
                tuple.mapper.Map<TemporaryExposureKeyGatewayDto, TemporaryExposureKey>(tuple.sourceKey);

            mappingAction.Should().Throw<AutoMapperMappingException>();
        }

        [Test]
        public void MapDto_ToEntity_ShouldHaveSharingAsFalse()
        {
            //Arrange
            var dto = new TemporaryExposureKeyBatchDto()
            {
                keys = new List<Core.Models.TemporaryExposureKeyDto>
                {
                    new Core.Models.TemporaryExposureKeyDto(){},
                    new Core.Models.TemporaryExposureKeyDto(){}
                }
            };
            IExposureKeyMapper mapper = new ExposureKeyMapper();

            //Act
            var entities = mapper.FromDtoToEntity(dto);

            //Assert
            entities.Should().NotBeNull();
            entities.Should().NotBeEmpty();
            entities.Select(ent => ent.SharingConsentGiven).Should().AllBeEquivalentTo(false);
        }

        [Test]
        public void MapDtoWithSharingTrue_ToEntity_ShouldHaveSharingTrue()
        {
            //Arrange
            var dto = new TemporaryExposureKeyBatchDto()
            {
                keys = new List<Core.Models.TemporaryExposureKeyDto>
                {
                    new Core.Models.TemporaryExposureKeyDto() { },
                    new Core.Models.TemporaryExposureKeyDto() { }
                },
                sharingConsentGiven = true
            };
            IExposureKeyMapper mapper = new ExposureKeyMapper();

            //Act
            var entities = mapper.FromDtoToEntity(dto);

            //Assert
            entities.Should().NotBeNull();
            entities.Should().NotBeEmpty();
            entities.Select(ent => ent.SharingConsentGiven).Should().AllBeEquivalentTo(true);
        }

        private (IMapper mapper, TemporaryExposureKeyGatewayDto sourceKey, Country country) CreateMapperSourceKeyAndCountry()
        {
            // .: Arrange
            var poland = new Country() { Id = 1, Code = "PL" };
            var denmark = new Country() { Id = 2, Code = "DK" };
            var germany = new Country() { Id = 3, Code = "GR" };
            var netherlands = new Country() { Id = 4, Code = "NL" };

            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);
            countryRepositoryMock
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code == "PL")))
                .Returns(poland);
            countryRepositoryMock
                .Setup(repo => repo.FindByIsoCode(It.Is<string>(code => code == NonExistingCountryCode)))
                .Returns(null as Country);

            var mockedVisitedCountries = new List<Country>
            {
                denmark,
                germany,
                netherlands
            };

            countryRepositoryMock
                .Setup(repo => repo.FindByIsoCodes(It.IsAny<IList<string>>()))
                .Returns(
                    (IList<string> param) => mockedVisitedCountries.Where(country => param.Contains(country.Code))
                );

            var mapper = CreateAutoMapperWithDependencies(countryRepositoryMock.Object);

            (var sourceKey, var country) = CreateMockedUeGatewayExposureKey();

            return (mapper, sourceKey, country);
        }

        private (TemporaryExposureKeyGatewayDto, Country) CreateMockedUeGatewayExposureKey()
        {
            var countries = new List<string>()
            {
                "NL", "DK", "GR",
            };

            var originCountry = new Country()
            {
                Id = 1,
                Code = "PL",
                PullingFromGatewayEnabled = true,
            };

            var mockedKey = new TemporaryExposureKeyGatewayDto()
            {
                KeyData = Encoding.UTF8.GetBytes("keyToday"),
                RollingPeriod = 144,
                RollingStartIntervalNumber = (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMinutes / 10,
                TransmissionRiskLevel = (int)RiskLevel.RISK_LEVEL_LOW,
                Origin = originCountry.Code,
                VisitedCountries = countries.OrderBy(c => c).ToList(),
                ReportType = "RECURSIVE"
            };

            return (mockedKey, originCountry);
        }

        private TemporaryExposureKey CreateMockedExposureKey()
        {
            var countries = new (string, string)[] { ("PL", "Poland"), ("GR", "German"), ("NL", "Netherlands ") };

            var mockEntity = new TemporaryExposureKey() { Id = Guid.NewGuid() };
            var originIntersection = new TemporaryExposureKeyCountry();

            // setup visitedCountries
            var visitedCountries = new List<TemporaryExposureKeyCountry>();
            for (int i = 0; i < countries.Length; ++i)
            {
                (string cCode, string cName) = countries[i];
                var country = new Country()
                {
                    Id = i,
                    Code = cCode,
                    PullingFromGatewayEnabled = true,
                    TemporaryExposureKeyCountries = new List<TemporaryExposureKeyCountry> { new TemporaryExposureKeyCountry() { TemporaryExposureKey = mockEntity } }
                };
                var cIntersection = new TemporaryExposureKeyCountry()
                {
                    Country = country,
                    CountryId = country.Id,
                    TemporaryExposureKey = mockEntity,
                    TemporaryExposureKeyId = mockEntity.Id
                };
                visitedCountries.Add(cIntersection);
            }

            // setup origin
            var originCountry = new Country()
            {
                Id = 1,
                Code = "DK",
                PullingFromGatewayEnabled = true,
                TemporaryExposureKeyCountries = new List<TemporaryExposureKeyCountry> { originIntersection },
            };

            originIntersection.Country = originCountry;
            originIntersection.CountryId = originCountry.Id;
            originIntersection.TemporaryExposureKey = mockEntity;
            originIntersection.TemporaryExposureKeyId = mockEntity.Id;

            mockEntity.KeyData = Encoding.ASCII.GetBytes("keyToday");
            mockEntity.RollingPeriod = 144;
            mockEntity.RollingStartNumber = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMinutes / 10;
            mockEntity.CreatedOn = DateTime.UtcNow;
            mockEntity.TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW;
            mockEntity.Origin = originCountry;
            mockEntity.VisitedCountries = visitedCountries;
            mockEntity.DaysSinceOnsetOfSymptoms = 4;

            return mockEntity;
        }

        private IMapper CreateAutoMapperWithDependencies(ICountryRepository repository)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();

            services.AddSingleton(repository);
            services.AddAutoMapper(typeof(TemporaryExposureKeyToEuGatewayMapper));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMapper>();

        }
    }
}
