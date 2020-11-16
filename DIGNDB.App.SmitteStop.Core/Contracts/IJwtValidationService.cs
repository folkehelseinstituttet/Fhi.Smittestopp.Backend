namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IJwtValidationService
    {
        bool IsTokenValid(string validToken);
    }
}