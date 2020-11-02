using System;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class DownloadKeysFromGatewayJobConfig : PeriodicJobBaseConfig
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct UploadKeysAgeLimitInDays")]
        public int MaximumNumberOfDaysBack { get; set; } = 6;
    }
}
