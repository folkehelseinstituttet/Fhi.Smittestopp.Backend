using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.API
{
    public class LogValidationRulesConfig
    {
        [Required(AllowEmptyStrings = false)]
        public string SeverityRegex { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PositiveNumbersRegex { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BuildVersionRegex { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string OperationSystemRegex { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string DeviceOSVersionRegex { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int MaxTextFieldLength { get; set; }
    }
}
