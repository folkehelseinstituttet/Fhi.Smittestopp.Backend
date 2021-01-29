using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Db;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class KeyValidatorTests
    {

        public const bool Enabled = true;
        public IKeyValidator _keyValidator { get; set; }
        public TemporaryExposureKey _key { get; set; }
        public CountryMockFactory _countryFactory { get; set; }
        public MockRandomGenerator _rndGenerator { get; set; }
        public ExposureKeyMock _exposureKeyMock { get; set; }
        public Mock<IEpochConverter> _epochConverter { get; set; }


        [SetUp]
        public void Init()
        {

            _epochConverter = new Mock<IEpochConverter>(MockBehavior.Strict);
            SetupEpochConverterMock();
            _keyValidator = new KeyValidator(_epochConverter.Object);
            _countryFactory = new CountryMockFactory();
            _rndGenerator = new MockRandomGenerator();
            _exposureKeyMock = new ExposureKeyMock();
            _key = _exposureKeyMock.CreateMockedExposureKey();
        }

        [Test]
        public void ValidKeysHaveExactly16bytes()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);

            _key.KeyData = _rndGenerator.GenerateKeyData(_rndGenerator.GetIntFromInterval(0, 15));
            Assert.IsFalse(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
            _key.KeyData = _rndGenerator.GenerateKeyData(_rndGenerator.GetIntFromInterval(16, 10000));
            Assert.IsFalse(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
            _key.KeyData = _rndGenerator.GenerateKeyData(16);
            Assert.IsTrue(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
        }
        [Test]
        public void CountryMustNotBeDK()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);

            _key.Origin = _countryFactory.GenerateCountry(7, "DK");
            Assert.IsFalse(_keyValidator.ValidateKeyGateway(_key, out errorMessage));

            _key.Origin = _countryFactory.GenerateCountry(21, "DK");
            Assert.IsFalse(_keyValidator.ValidateKeyGateway(_key, out errorMessage));

            _key.Origin = _countryFactory.GenerateCountry(21, "PL");
            Assert.IsTrue(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
        }

        private const int MinInvalidRange = 2986;
        private const int MaxInvalidRange = 3014;

        private static readonly IEnumerable<int> InvalidRange =
            Enumerable.Range(MinInvalidRange, MaxInvalidRange - MinInvalidRange + 1);

        [Test]
        public void DaysSinceOnsetOfSymptomsMustNotBeBetween2986And3014(
            [ValueSource(nameof(InvalidRange))] int? daysSinceOnsetOfSymptoms)
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);

            _key.DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;

            var validationResult = _keyValidator.ValidateKeyGateway(_key, out errorMessage);
            validationResult.Should().BeFalse();
        }

        [TestCase(0)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void DaysSinceOnsetOfSymptomsMustBeBetweenValidRange(int? daysSinceOnsetOfSymptoms)
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);

            _key.DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;

            var validationResult = _keyValidator.ValidateKeyGateway(_key, out errorMessage);
            validationResult.Should().BeTrue();
        }

        [Test]
        public void OriginGatewayServiceMustBeEnabled()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);
            _key.Origin = _countryFactory.GenerateCountry(21, "PL", !Enabled);
            Assert.IsFalse(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
            _key.Origin = _countryFactory.GenerateCountry(21, "PL", Enabled);
            Assert.IsTrue(_keyValidator.ValidateKeyGateway(_key, out errorMessage));
        }

        [Test]
        public void APIKeysShouldBeExactly16ByteLong()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);
            _key.KeyData = _rndGenerator.GenerateKeyData(_rndGenerator.GetIntFromInterval(0, 15));
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));
            _key.KeyData = _rndGenerator.GenerateKeyData(_rndGenerator.GetIntFromInterval(16, 10000));
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));
            _key.KeyData = _rndGenerator.GenerateKeyData(16);
            Assert.IsTrue(_keyValidator.ValidateKeyAPI(_key, out errorMessage));

        }
        [Test]
        public void RollingPeriodShouldBeInRangeOf10MinutesAnd24Hours()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);
            _key.RollingPeriod = _rndGenerator.GetIntFromInterval(Int32.MinValue, 0);
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));
            _key.RollingPeriod = _rndGenerator.GetIntFromInterval(145, Int32.MaxValue);
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));
            _key.RollingPeriod = _rndGenerator.GetIntFromInterval(1, 144);
            Assert.IsTrue(_keyValidator.ValidateKeyAPI(_key, out errorMessage));

        }
        [Test]
        public void RollingStartNumberShouldNotBeInTheFutureAndEarlierThan14DaysAgo()
        {
            var errorMessage = String.Empty;
            _exposureKeyMock.ResetKeyData(_key);
            Assert.IsTrue(_keyValidator.ValidateKeyAPI(_key, out errorMessage));

            _key.RollingStartNumber = _epochConverter.Object.ConvertToEpoch(DateTime.Today.Date.AddDays(_rndGenerator.GetIntFromInterval(-14, 0)));
            Assert.IsTrue(_keyValidator.ValidateKeyAPI(_key, out errorMessage));

            _key.RollingStartNumber = _epochConverter.Object.ConvertToEpoch(DateTime.Today.Date.AddDays(_rndGenerator.GetIntFromInterval(-100, -15)));
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));

            _key.RollingStartNumber = _epochConverter.Object.ConvertToEpoch(DateTime.Today.Date.AddDays(_rndGenerator.GetIntFromInterval(1, 100)));
            Assert.IsFalse(_keyValidator.ValidateKeyAPI(_key, out errorMessage));


        }


        public void SetupEpochConverterMock()
        {
            _epochConverter.Setup(mock => mock.ConvertToEpoch(It.IsAny<DateTime>())).Returns((DateTime date) =>
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64((date - epoch).TotalSeconds);
            });
            _epochConverter.Setup(mock => mock.ConvertFromEpoch(It.IsAny<long>())).Returns((long epochTime) =>
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddSeconds(epochTime);
            });
        }

    }
}
