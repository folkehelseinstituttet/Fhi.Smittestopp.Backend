namespace DIGNDB.App.SmitteStop.Domain
{
    public class JwtValidationRules
    {
        public string ClientId { get; set; }
        public string SupportedAlgorithm { get; set; }
        public string Issuer { get; set; }
    }
}