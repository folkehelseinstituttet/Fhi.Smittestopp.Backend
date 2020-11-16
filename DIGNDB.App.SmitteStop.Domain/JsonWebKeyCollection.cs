using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace DIGNDB.App.SmitteStop.Domain
{
    public class JsonWebKeyCollection
    {
        public List<JsonWebKey> Keys { get; set; }
    }
}