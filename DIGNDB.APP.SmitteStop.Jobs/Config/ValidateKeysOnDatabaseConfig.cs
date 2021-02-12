using System;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class ValidateKeysOnDatabaseConfig : PeriodicJobBaseConfig
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct BatchSize")]
        public int BatchSize { get; set; }
    }
}
