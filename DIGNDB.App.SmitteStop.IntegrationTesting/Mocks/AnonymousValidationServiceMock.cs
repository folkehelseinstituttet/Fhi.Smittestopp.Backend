using DIGNDB.App.SmitteStop.API.Contracts;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class AnonymousValidationServiceMock : IAnonymousTokenValidationService
    {
        private bool TokenIsValid { get; }

        public AnonymousValidationServiceMock(bool isTokenValid)
        {
            TokenIsValid = isTokenValid;
        }

        public Task<bool> IsTokenValid(string anonymousToken)
        {
            return Task.FromResult(TokenIsValid);
        }
    }
}
