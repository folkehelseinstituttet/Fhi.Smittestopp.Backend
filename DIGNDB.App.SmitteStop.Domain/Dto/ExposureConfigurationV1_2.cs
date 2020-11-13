using DIGNDB.App.SmitteStop.Domain.Dto;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Core.Models
{
    public class ExposureConfigurationV1_2
    {
        [Required]
        public ExposureConfiguration Configuration { get; set; }

        [Required]
        public AttenuationBucketsParams AttenuationBucketsParams { get; set; }
    }
}
