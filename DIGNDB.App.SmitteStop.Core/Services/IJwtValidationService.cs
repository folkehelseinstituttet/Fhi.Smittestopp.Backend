namespace DIGNDB.App.SmitteStop.Core.Services
{
    public interface IJwtValidationService
    {
        bool IsTokenValid(string validToken);
    }
}