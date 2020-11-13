using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface IJwtTokenRepository
    {
        bool HasTokenBeenUsed(string tokenId);
        void RemoveExpiredTokens();
        void InsertValidToken(JwtToken token);
    }
}