using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public class KeyValidationConfiguration
    {
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int OutdatedKeysDayOffset { get; set; }

        [Required]
        public PackageNameConfig PackageNames { get; set; }
    }
}
