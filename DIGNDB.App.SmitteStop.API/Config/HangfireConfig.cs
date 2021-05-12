using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class HangFireConfig
    {
        [Required(AllowEmptyStrings = false)]
        public string HangFireConnectionString { get; set; }
    }
}