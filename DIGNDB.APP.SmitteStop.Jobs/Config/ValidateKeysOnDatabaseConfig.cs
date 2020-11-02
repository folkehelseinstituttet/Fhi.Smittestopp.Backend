using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class ValidateKeysOnDatabaseConfig : PeriodicJobBaseConfig
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct BatchSize")]
        public int BatchSize { get; set; }
    }
}
