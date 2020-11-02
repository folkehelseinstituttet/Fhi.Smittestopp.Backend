using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class DailyMaintenanceCheckJobConfig : PeriodicJobBaseConfig
    {
        [Required]
        public GatewayDowloadCheckConfig GatewayDownloadCheck { get; set; }
    }

    public class GatewayDowloadCheckConfig
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1} for DayToCheckBeforeTodayOffset")]
        public int? DayToCheckBeforeTodayOffset { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1} for RiseErrorWhenNumberOfKeysAreBelowNumber")]
        public int? RiseErrorWhenNumberOfKeysAreBelowNumber { get; set; }
    }
}
