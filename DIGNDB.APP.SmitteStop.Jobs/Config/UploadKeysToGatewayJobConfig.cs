using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class UploadKeysToGatewayJobConfig : PeriodicJobBaseConfig
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct BatchSize")]
        public int BatchSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct UploadKeysAgeLimitInDays")]
        public int UploadKeysAgeLimitInDays { get; set; }
    }
}
