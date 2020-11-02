using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class LoggingConfig
    {
        [Required]
        public EventLogConfig EventLog { get; set; }
    }
}
