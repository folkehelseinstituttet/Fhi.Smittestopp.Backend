using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class LoggingConfig
    {
        [Required]
        public EventLogConfig EventLog { get; set; }
    }
}
