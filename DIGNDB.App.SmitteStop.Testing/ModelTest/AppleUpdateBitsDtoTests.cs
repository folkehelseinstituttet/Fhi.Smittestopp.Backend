using NUnit.Framework;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class AppleUpdateBitsDtoTests
    {
        AppleUpdateBitsDto _dto;
        [SetUp]
        public void initDto()
        {
            _dto = new AppleUpdateBitsDto();
        }
        [Test]
        public void AppleUpdateBitsDto_SetDeviceToken_ShouldGetCorrectValue()
        {
            var expect = "token";
            _dto.device_token = expect;
            Assert.AreEqual(expect, _dto.device_token);
        }
        [Test]
        public void AppleUpdateBitsDto_SetTimestamp_ShouldGetCorrectValue()
        {
            var expect = 123;
            _dto.timestamp = expect;
            Assert.AreEqual(expect, _dto.timestamp);
        }
        [Test]
        public void AppleUpdateBitsDto_SetTransactionId_ShouldGetCorrectValue()
        {
            var expect = "transactionID";
            _dto.transaction_id = expect;
            Assert.AreEqual(expect, _dto.transaction_id);
        }
        [Test]
        public void AppleUpdateBitsDto_SetBit0_ShouldGetCorrectValue()
        {
            _dto.bit0 = true;
            Assert.IsTrue(_dto.bit0);
        }
        [Test]
        public void AppleUpdateBitsDto_SetBit1_ShouldGetCorrectValue()
        {
            _dto.bit1 = false;
            Assert.IsFalse(_dto.bit1);
        }
    }
}
