using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class DailySummaryExposureConfiguration
    {
        [Required]
        public DailySummaryConfiguration.DailySummaryConfiguration DailySummaryConfiguration { get; set; }

        [Required]
        public double MaximumScoreThreshold { get; set; }
    }
}
