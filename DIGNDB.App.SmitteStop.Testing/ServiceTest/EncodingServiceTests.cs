using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class EncodingServiceTests
    {        
        private readonly IEncodingService _encodingService;

        public EncodingServiceTests()
        {
            _encodingService = new EncodingService();
        }

        [TestCase("ABC", ExpectedResult = "QUJD")]
        [TestCase("111111111111", ExpectedResult = "MTExMTExMTExMTEx")]
        [TestCase(null, ExpectedResult = null)]
        public string TestEncodeToBase64(string stringToEncode)
        {
            return _encodingService.EncodeToBase64(stringToEncode);
        }
        
        [TestCase("QUJD", ExpectedResult = "ABC")]
        [TestCase("MTExMTExMTExMTEx", ExpectedResult = "111111111111")]
        [TestCase(null, ExpectedResult = null)]
        public string TestDecodeFromBase64(string stringToDecode)
        {
            return _encodingService.DecodeFromBase64(stringToDecode);
        }
    }
}