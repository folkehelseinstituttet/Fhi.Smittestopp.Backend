using DIGNDB.App.SmitteStop.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.API
{
    public class AppSettingsConfig : IOriginSpecificSettings, IPackageBuilderConfig
    {
        [Required(AllowEmptyStrings = false)]
        public string OriginCountryCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AppleQueryBitsUrl { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AppleUpdateBitsUrl { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string PrivateKey { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string AppleKeyID { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string AppleDeveloperAccount { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string AuthJWTPublicKey { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string AuthorizationMobile { get; set; }

        // [Required(AllowEmptyStrings = false)] //TODO uncomment when will be provided by mobile team
        public string CertificateThumbprint { get; set; }

        [Required]
        public TimeSpan PreviousDayFileCaching { get; set; }

        [Required]
        public TimeSpan CurrentDayFileCaching { get; set; }

        public bool DeviceVerificationEnabled { get; set; }

        [Required]
        public string LogsApiPath { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int MaxKeysPerFile { get; set; } = 750000;

        public int CacheMonitorTimeout { get; set; } = 100;

        public bool LogEndpointOverride { get; set; }

        [Range(minimum:0, maximum:int.MaxValue)]
        public int FetchCommandTimeout { get; set; }

        [Required]
        public List<string> DeprecatedVersions { get; set; } = new List<string>();

        public bool EnableCacheOverride { get; set; }
    }
}
