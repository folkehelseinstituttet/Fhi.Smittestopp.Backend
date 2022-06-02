using DIGNDB.App.SmitteStop.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.App.SmitteStop.API
{
    /// <summary>
    /// Class for appsettings configuration values
    /// </summary>
    public class AppSettingsConfig : IOriginSpecificSettings, IZipPackageBuilderConfig
    {
        [Required(AllowEmptyStrings = false)]
        public string OriginCountryCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AuthorizationMobile { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ZipCertificatePath { get; set; }

        [Required]
        public TimeSpan PreviousDayFileCaching { get; set; }

        [Required]
        public TimeSpan CurrentDayFileCaching { get; set; }

        public bool DeviceVerificationEnabled { get; set; }

        [Required]
        public string LogsApiPath { get; set; }

        [Required]
        public string LogsJobsPath { get; set; }

        [Required]
        public string LogsMobilePath { get; set; }
        
        [Range(minimum: 100, maximum: int.MaxValue)]
        public long LogFileSizeLimitBytes { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int MaxKeysPerFile { get; set; } = 750000;

        public int CacheMonitorTimeout { get; set; } = 100;

        public bool LogEndpointOverride { get; set; }

        [Range(minimum: 0, maximum: int.MaxValue)]
        public int FetchCommandTimeout { get; set; }

        [Required]
        public List<string> DeprecatedVersions { get; set; } = new List<string>();

        [Required]
        public string ZipFilesFolder { get; set; }

        [Required]
        public GitHubSettings GitHubSettings { get; set; }

        [Required]
        public HealthCheckSettings HealthCheckSettings { get; set; }

        public bool EnableCacheOverride { get; set; }
    }

    public class GitHubSettings
    {
        [Required] 
        public string GitHubStatisticsZipFileFolder { get; set; }
        /// <summary>
        /// Appsettings value used when accessing covid statistics endpoint from GitHub
        /// </summary>
        [Required]
        public string AuthorizationGitHub { get; set; }
        [Required]
        public int DaysToSaveFiles { get; set; }
        [Required]
        public string FileNameDatePattern { get; set; }
        [Required]
        public string FileNameDateParsingFormat { get; set; }

        [Required]
        public string TestedFileNamePattern { get; set; }
        [Required]
        public string HospitalAdmissionFileNamePattern { get; set; }
        [Required]
        public string VaccinationFileNamePattern { get; set; }
        [Required]
        public string TimeLocationFileNamePattern { get; set; }
        [Required]
        public string LocationFileNamePattern { get; set; }
        [Required]
        public string DeathByTimeFileNamePattern { get; set; }
    }

    public class HealthCheckSettings
    {
        /// <summary>
        /// Appsettings value holding name of server 1
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Server1Name { get; set; }

        /// <summary>
        /// Appsettings value used when accessing health check endpoints
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string AuthorizationHealthCheck { get; set; }

        /// <summary>
        /// Appsettings value for numbers today earliest call time
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public int NumbersTodayCallAfter24Hour { get; set; }

        /// <summary>
        /// Appsettings value for zip files earliest call time
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public int ZipFilesCallAfter24Hour { get; set; }

        /// <summary>
        /// Appsettings value for api log file name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ApiRegex { get; set; }

        /// <summary>
        /// Appsettings value jobs log file name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string JobsRegex { get; set; }

        /// <summary>
        /// Appsettings value mobile logs file name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string MobileRegex { get; set; }

        /// <summary>
        /// Appsettings value log files date pattern
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string LogFilesDatePattern { get; set; }

        /// <summary>
        /// Appsettings value mobile log files date pattern
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string MobileLogFilesDatePattern { get; set; }

        /// <summary>
        /// Appsettings value for No. of jobs that should be enabled
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public int NoOfEnabledJobs { get; set; }

        /// <summary>
        /// Appsettings value for jobs IDs that is expected to be enabled
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public List<string> EnabledJobIds { get; set; }
    }
}
