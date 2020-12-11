using DIGNDB.App.SmitteStop.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.API
{
    public class Logging
    {
        public LogLevelType LogLevel { get; set; }

        public class LogLevelType
        {
            public string Default { get; set; }
        }
    }
}
