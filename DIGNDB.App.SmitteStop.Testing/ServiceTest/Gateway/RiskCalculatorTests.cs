using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class RiskCalculatorTests
    {
        public RiskCalculator _riskCalculator { get; set; }
        public ExposureKeyMock _exposureKeyMock { get; set; }
        public TemporaryExposureKey _key { get; set; }
        public MockRandomGenerator _rndGenerator { get; set; }

        [SetUp]
        public void Init()
        {
            _riskCalculator = new RiskCalculator();
            _exposureKeyMock = new ExposureKeyMock();
            _rndGenerator = new MockRandomGenerator();

            _key = _exposureKeyMock.CreateMockedExposureKey();

        }

        [Test]
        public void TransmissionRiskLevelShouldBeSetToHighestByDefault()
        {

            _exposureKeyMock.ResetKeyData(_key);
            _key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(_key);
            Assert.That(_key.TransmissionRiskLevel == RiskLevel.RISK_LEVEL_HIGHEST);

        }

        [Test]
        public void TransmissionLevelIsProperlyCalculated()
        {
            _exposureKeyMock.ResetKeyData(_key);
            _key.DaysSinceOnsetOfSymptoms = _rndGenerator.GetIntFromInterval(0,15);
            _key.TransmissionRiskLevel = _riskCalculator.CalculateRiskLevel(_key);
            Assert.That(_key.TransmissionRiskLevel<=RiskLevel.RISK_LEVEL_HIGHEST, $"calculated risk level: {_key.TransmissionRiskLevel}");
        }


    }
}
