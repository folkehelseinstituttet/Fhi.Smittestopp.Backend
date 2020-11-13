using System;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public class JwtToken
    {
        public string Id { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}