using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(DigNDB_SmittestopContext db)
        {
            var tokens = GetTokens();
            db.JwtToken.AddRange(tokens);
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DigNDB_SmittestopContext db)
        {
            db.JwtToken.RemoveRange(db.JwtToken);
            InitializeDbForTests(db);
        }

        public static List<JwtToken> GetTokens()
        {
            return new List<JwtToken>
            {
                new JwtToken{ Id = "someRandomString", ExpirationTime = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10))},
            };
        }
    }
}
