namespace FederationGatewayApi.Models
{
    public class BatchStatus
    {
        public bool NextBatchExists { get; internal set; }

        public int KeysProcessed { get; internal set; }

        public int KeysToSend { get; internal set; }

        public int KeysSent { get; internal set; }

        public bool ProcessedSuccessfully { get; internal set; }
    }
}
