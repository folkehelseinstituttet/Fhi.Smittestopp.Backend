using System.Diagnostics.CodeAnalysis;

namespace DIGNDB.App.SmitteStop.Domain.Settings
{
    public class GatewayDownloadState
    {
        /// <summary>
        /// Last synchronization date saved as a DataTime.Ticks
        /// </summary>
        [AllowNull]
        public long? LastSyncDate { get; set; }


        /// <summary>
        /// Last batch number downloaded for current LastSyncDate
        /// </summary>
        [AllowNull]
        public string LastSyncedBatchTag { get; set; }
    }
}
