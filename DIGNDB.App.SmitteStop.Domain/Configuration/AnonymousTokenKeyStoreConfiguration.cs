using System;

namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public class AnonymousTokenKeyStoreConfiguration
    {
        public string CertificateThumbPrint { get; set; }
        public TimeSpan KeyRotationInterval { get; set; } = TimeSpan.FromDays(3);
        public TimeSpan KeyRotationRollover { get; set; } = TimeSpan.FromHours(1);
    }
}
