using System.ComponentModel.DataAnnotations;

namespace FederationGatewayApi.Config
{
    public class EuGatewayConfig
    {
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }

        public string UrlNormalized => Url.TrimEnd('/') + "/";

        [Required(AllowEmptyStrings = false)]
        public string AuthenticationCertificateFingerprint { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SigningCertificateFingerprint { get; set; }
    }
}
