using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ImportantInfoServiceTests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<ILogger<ImportantInfoService>> _logger;

        private ImportantInfoService _service;

        private const string BannerConfigurationFile = "./ServiceTest/Files/BannerConfiguration.json";
        private const string BannerConfigurationFileConfig = "BannerConfigurationFile";

        [SetUp]
        public void Init()
        {
            _configuration = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<ImportantInfoService>>();
            _service = new ImportantInfoService(_logger.Object, _configuration.Object);
        }

        [Test]
        public void ConfigFileExists_FileExists_ReturnsTrue()
        {
            _configuration.Setup(config => config[BannerConfigurationFileConfig])
                .Returns(BannerConfigurationFile);

            bool exists = _service.ConfigFileExists();

            Assert.True(exists);
        }

        [Test]
        public void ConfigFileExists_FileDoesNotExists_ReturnsFalse()
        {
            bool exists = _service.ConfigFileExists();

            Assert.False(exists);
        }

        [Test]
        public void ParseConfig_LanguageIsPresent_ReturnsResponse()
        {
            _configuration.Setup(config => config[BannerConfigurationFileConfig])
                .Returns(BannerConfigurationFile);

            ImportantInfoResponse response = _service.ParseConfig(new ImportantInfoRequest() { lang = "NB" });

            Assert.NotNull(response);
            Assert.NotNull(response.text);
        }

        [Test]
        public void ParseConfig_LanguageIsNotPresent_ReturnsResponseWithEmptyText()
        {
            _configuration.Setup(config => config[BannerConfigurationFileConfig])
                .Returns(BannerConfigurationFile);

            ImportantInfoResponse response = _service.ParseConfig(new ImportantInfoRequest() { lang = "SE" });

            Assert.NotNull(response);
            Assert.Null(response.text);
        }
    }
}
