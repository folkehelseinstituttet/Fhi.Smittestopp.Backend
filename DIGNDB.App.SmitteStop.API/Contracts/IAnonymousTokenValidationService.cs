using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Contracts
{
    public interface IAnonymousTokenValidationService
    {
        Task<bool> IsTokenValid(string anonymousToken);
    }
}