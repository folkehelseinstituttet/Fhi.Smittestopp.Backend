namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IJwtValidationService
    {
        bool IsTokenValid(string validToken);
    }
}