using System.Diagnostics.CodeAnalysis;

namespace DIGNDB.App.SmitteStop.Domain.Settings
{
    public class GatewayUploadState
    {
        [AllowNull]
        public long? CreationDateOfLastUploadedKey { get; set; }

        public int NumberOfKeysProcessedFromTheLastCreationDate { get; set; }
    }
}
