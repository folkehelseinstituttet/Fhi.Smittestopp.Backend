using System.Text.Json.Serialization;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class CountryDto
    {
        public long Id { get; set; }

        [JsonPropertyName("name_EN")]
        public string NameInEnglish { get; set; }

        [JsonPropertyName("name_DA")]
        public string NameInDanish { get; set; }
        public string Code { get; set; }
    }
}