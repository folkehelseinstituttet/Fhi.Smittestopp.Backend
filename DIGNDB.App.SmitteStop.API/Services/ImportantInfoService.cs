using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using DIGNDB.App.SmitteStop.API.Config;
using DIGNDB.App.SmitteStop.API.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class ImportantInfoService : IImportantInfoService
    {
        private readonly ILogger<ImportantInfoService> _logger;
        private IConfiguration _configuration; 

        public ImportantInfoService( ILogger<ImportantInfoService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool ConfigFileExists()
        {
            return File.Exists(_configuration["BannerConfigurationFile"]);
        }

        public string GetColor()
        {
            using (StreamReader r = new StreamReader(_configuration["BannerConfigurationFile"]))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<ImportantInfoList>(json);
                return items.color;
            }
        }

        public Message ParseConfig(ImportantInfoRequest request)
        {
            ImportantInfoList items;
            using (StreamReader r = new StreamReader(_configuration["BannerConfigurationFile"]))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<ImportantInfoList>(json);

                var item = items.message.SingleOrDefault(m => m.lang == request.lang);
                return item;
            }
        }
    }
}
