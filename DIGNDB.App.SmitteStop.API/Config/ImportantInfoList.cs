namespace DIGNDB.App.SmitteStop.API.Config
{

    public class ImportantInfoList
    {
        public Message[] message { get; set; }
        public string color { get; set; }
    }

    public class Message
    {
        public string lang { get; set; }
        public string text { get; set; }
        public string creationDate { get; set; }
        public string expirationDate { get; set; }
    }
}
