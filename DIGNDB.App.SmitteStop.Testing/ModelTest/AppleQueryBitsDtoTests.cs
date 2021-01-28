using DIGNDB.App.SmitteStop.Domain.Dto;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class AppleQueryBitsDtoTests
    {
        AppleQueryBitsDto _dto;

        [SetUp]
        public void initDto()
        {
            _dto = new AppleQueryBitsDto();
        }

        [Test]
        public void AppleQueryBitsDto_SetDeviceToken_ShouldReturnCorrectValue()
        {
            var expect = "token";
            _dto.device_token = expect;
            Assert.AreEqual(expect, _dto.device_token);
        }

        [Test]
        public void AppleQueryBitsDto_SetDeviceTimeStamp_ShouldReturnCorrectValue()
        {
            var expect = 123;
            _dto.timestamp = expect;
            Assert.AreEqual(expect, _dto.timestamp);
        }

        [Test]
        public void AppleQueryBitsDto_SetTransactionID_ShouldReturnCorrectValue()
        {
            var expect = "ID";
            _dto.transaction_id = expect;
            Assert.AreEqual(expect, _dto.transaction_id);
        }
    }
}
