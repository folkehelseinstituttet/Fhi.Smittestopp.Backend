using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class CountryMockFactory
    {

        public Country GenerateCountry(int id, string code, bool gatewayEnabled = true)
        {
            var originCountry = new Country()
            {
                Id = id,
                Code = code,
                PullingFromGatewayEnabled = gatewayEnabled
            };
            return originCountry;
        }

    }
}
