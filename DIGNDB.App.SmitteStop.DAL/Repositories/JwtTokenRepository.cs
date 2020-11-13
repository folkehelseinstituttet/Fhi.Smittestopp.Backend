using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using System.Linq;
using System;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class JwtTokenRepository : GenericRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepository(DigNDB_SmittestopContext context) : base(context) { }

        public bool HasTokenBeenUsed(string tokenId)
        {
            return _dbSet.Any(token => token.Id == tokenId);
        }

        public void InsertValidToken(JwtToken token)
        {
            Insert(token);

            Save();
        }

        public void RemoveExpiredTokens()
        {
            var expiredTokens = Get(token => token.ExpirationTime <= DateTime.Now).ToList();

            foreach (var expiredToken in expiredTokens)
            {
                Delete(expiredToken);
            }

            if (expiredTokens.Any())
                Save();
        }
    }
}
