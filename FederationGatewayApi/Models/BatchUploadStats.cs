namespace FederationGatewayApi.Models
{
    public class BatchUploadStats
    {
        public long CurrentBatchNumber { get;  set; }
        public int TotalKeysProcessed { get; internal set; }
        public int TotalKeysSent { get; internal set; }
    }
}
