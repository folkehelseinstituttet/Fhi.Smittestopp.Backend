using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class JwtAuthorization
    {
        public JwtValidationRules JwtValidationRules { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JwkUrl { get; set; }
    }
}