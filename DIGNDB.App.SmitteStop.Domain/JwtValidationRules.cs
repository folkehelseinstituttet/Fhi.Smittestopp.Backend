using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class JwtValidationRules
    {
        [Required(AllowEmptyStrings = false)]
        public string ClientId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SupportedAlgorithm { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Issuer { get; set; }
    }
}