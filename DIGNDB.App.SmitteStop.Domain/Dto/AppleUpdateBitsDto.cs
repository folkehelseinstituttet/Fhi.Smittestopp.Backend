namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class AppleUpdateBitsDto
    {
        public string device_token { get; set; }
        public long timestamp { get; set; }
        public string transaction_id { get; set; }
        public bool bit0 { get; set; }
        public bool bit1 { get; set; }
    }
}
