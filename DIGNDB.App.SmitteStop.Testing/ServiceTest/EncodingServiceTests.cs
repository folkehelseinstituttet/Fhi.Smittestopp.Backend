using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Services;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class EncodingServiceTests
    {
        private const string ExampleString1 = "ABC";
        private const string ExampleString2 = "111111111111";
        private const string EncodedString1 = "QUJD";
        private const string EncodedString2 = "MTExMTExMTExMTEx";
        
        private readonly IEncodingService _encodingService;

        public EncodingServiceTests()
        {
            _encodingService = new EncodingService();
        }

        [TestCase(ExampleString1, ExpectedResult = EncodedString1)]
        [TestCase(ExampleString2, ExpectedResult = EncodedString2)]
        [TestCase(null, ExpectedResult = null)]
        public string TestEncodeToBase64(string stringToEncode)
        {
            return _encodingService.EncodeToBase64(stringToEncode);
        }
        
        [TestCase(EncodedString1, ExpectedResult = ExampleString1)]
        [TestCase(EncodedString2, ExpectedResult = ExampleString2)]
        [TestCase(null, ExpectedResult = null)]
        public string TestDecodeFromBase64(string stringToDecode)
        {
            return _encodingService.DecodeFromBase64(stringToDecode);
        }
    }
}