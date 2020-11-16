using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Core.Models
{
    public class AttenuationBucketsParams
    {
        [Required]
        public double? ExposureTimeThreshold { get; set; }

        [Required]
        public double? LowAttenuationBucketMultiplier { get; set; }

        [Required]
        public double? MiddleAttenuationBucketMultiplier { get; set; }

        [Required]
        public double? HighAttenuationBucketMultiplier { get; set; }
    }
}
