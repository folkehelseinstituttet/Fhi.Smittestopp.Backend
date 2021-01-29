using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class JsonWebKeyCollection
    {
        public List<JsonWebKey> Keys { get; set; }
    }
}