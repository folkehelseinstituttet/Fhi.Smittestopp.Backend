using DIGNDB.App.SmitteStop.Domain.Dto;
using NUnit.Framework;
using System.Net;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class AppleResponseDtoTests
    {
        AppleResponseDto _dto;
        [SetUp]
        public void initDto()
        {
            _dto = new AppleResponseDto();
        }
        [Test]
        public void AppleResponseDto_SetResponseCode_ShouldGetCorrectValue()
        {
            var expect = (HttpStatusCode)400;
            _dto.ResponseCode = expect;
            Assert.AreEqual(expect, _dto.ResponseCode);
        }
        [Test]
        public void AppleResponseDto_SetContent_ShouldGetCorrectValue()
        {
            var expect = "content";
            _dto.Content = expect;
            Assert.AreEqual(expect, _dto.Content);
        }
    }
}
