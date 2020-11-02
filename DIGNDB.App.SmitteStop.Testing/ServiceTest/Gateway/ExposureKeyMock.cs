using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using FederationGatewayApi.Models;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class ExposureKeyMock
    {
        public CountryMockFactory _countryFactory { get; set; }
        public MockRandomGenerator _rndGenerator { get; set; }
        public EpochConverter _epochConverter;
        public List<string> _countries;

        public static TemporaryExposureKey _potentialDuplicate01;
        public static TemporaryExposureKey _potentialDuplicate02;

        public int MockListLength;

        public ExposureKeyMock()
        {
            _countryFactory = new CountryMockFactory();
            _rndGenerator = new MockRandomGenerator();
            _epochConverter = new EpochConverter();
            _countries = new List<string>() {
                "AT", "BE", "BG","HR","CY", "CZ", "DK", "EE", "FI", "FR",
                "GR", "HU", "IE", "IT", "LV", "LT", "LU", "MT", "NL", "PL",
                "PT", "RO", "SK", "SI", "ES", "SE"};
            _potentialDuplicate01 = MockValidKey();
            _potentialDuplicate02 = MockValidKey();
            MockListLength = 5;
        }

        public TemporaryExposureKey CreateMockedExposureKey()
        {
            var mockEntity = new TemporaryExposureKey() { Id = Guid.NewGuid() };
            return mockEntity;
        }
        public void ResetKeyData(TemporaryExposureKey mockEntity)
        {

            var denmark = new Country() { Id = 2, Code = "DK" };
            var germany = new Country() { Id = 3, Code = "DE" };
            var netherlands = new Country() { Id = 4, Code = "NL" };
            var mockedVisitedCountries = new List<TemporaryExposureKeyCountry>
            {
                new TemporaryExposureKeyCountry() { Country = denmark, CountryId = denmark.Id },
                new TemporaryExposureKeyCountry() { Country = germany, CountryId = germany.Id },
                new TemporaryExposureKeyCountry() { Country = netherlands, CountryId = netherlands.Id},
            };
            mockEntity.RollingPeriod = 144;
            mockEntity.RollingStartNumber = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMinutes / 10;
            mockEntity.CreatedOn = DateTime.UtcNow;
            mockEntity.TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW;
            mockEntity.Origin = _countryFactory.GenerateCountry(21, "PL");
            mockEntity.KeyData = _rndGenerator.GenerateKeyData(16);
            mockEntity.RollingPeriod = 12;
            mockEntity.RollingStartNumber = _epochConverter.ConvertToEpoch(DateTime.Today);
            mockEntity.DaysSinceOnsetOfSymptoms = null;
            mockEntity.ReportType = ReportType.CONFIRMED_TEST;
            mockEntity.VisitedCountries = mockedVisitedCountries;

        }

        public TemporaryExposureKey MockValidKey()
        {
            var key = CreateMockedExposureKey();
            ResetKeyData(key);
            return key;
        }

        public TemporaryExposureKey MockInvalidKey()
        {
            var key = CreateMockedExposureKey();
            ResetKeyData(key);
            key.Origin = _countryFactory.GenerateCountry(7, "DK");
            key.KeyData = _rndGenerator.GenerateKeyData(13);
            key.RollingStartNumber = _epochConverter.ConvertToEpoch(DateTime.Today.AddDays(-20));
            return key;
        }

        public TemporaryExposureKeyGatewayDto MockTemporaryExposureKeyDTO()
        {
            var keyDTO = new TemporaryExposureKeyGatewayDto 
            {

                KeyData = _rndGenerator.GenerateKeyData(16),
                RollingPeriod = 10,
                RollingStartIntervalNumber = Convert.ToUInt32(_epochConverter.ConvertToEpoch(DateTime.Today)),
                Origin = GenerateOrigin(),
                ReportType = _rndGenerator.GetReportType(),
                VisitedCountries = new List<string> { "DK", "DE", "NL" }
            };
            return keyDTO;
        }
        public string GenerateOrigin()
        {
            int randomPosition = _rndGenerator.GetIntFromInterval(1, _countries.Count - 1);
            return _countries[randomPosition];
        }

        public void ResetCreatedOnAndId(List<TemporaryExposureKey> keys)
        {
            foreach (TemporaryExposureKey key in keys)
            {
                key.Id = Guid.Empty;
                key.CreatedOn = new DateTime();
            }
        }
        public List<TemporaryExposureKey> MockListOfTemporaryExposureKeys()
        {
            List<TemporaryExposureKey> keys = new List<TemporaryExposureKey>();

            for (int i = 0; i < MockListLength - 2; i++)
            {
                keys.Add(MockValidKey());
            }
            keys.Add(_potentialDuplicate01);
            keys.Add(_potentialDuplicate02);
            return keys;
        }
        public List<TemporaryExposureKeyGatewayDto> MockListOfTemporaryExposureKeyDto()
        {
            List<TemporaryExposureKeyGatewayDto> keys = new List<TemporaryExposureKeyGatewayDto>();
            for (int i = 0; i < MockListLength; i++)
            {
                keys.Add(MockTemporaryExposureKeyDTO());
            }
            return keys;
        }
    }
}




