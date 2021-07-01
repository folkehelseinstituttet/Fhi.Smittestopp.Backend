using DIGNDB.App.SmitteStop.API.Contracts;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class JwtValidationServiceMock : IJwtValidationService
    {
        private bool TokenIsValid { get; }

        public JwtValidationServiceMock(bool isTokenValid)
        {
            TokenIsValid = isTokenValid;
        }

        public bool IsTokenValid(string validToken)
        {
            return TokenIsValid;
        }
    }
}
