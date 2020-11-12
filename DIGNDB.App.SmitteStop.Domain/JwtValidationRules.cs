namespace DIGNDB.App.SmitteStop.Domain
{
    public class JwtValidationRules
    {
        public string AuthorizedPartyValue { get; set; }
        public string SupportedAlgorithm { get; set; }
        public string Issuer { get; set; }
    }
}