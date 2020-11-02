using FederationGatewayApi.Models;

namespace FederationGatewayApi.Contracts
{
    public interface IDaysSinceOnsetOfSymptomsDecoder
    {
        DaysSinceOnsetOfSymptomsResults Decode(int dsosInGatewayFormat);
    }
}
