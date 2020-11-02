namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IEncodingService
    {
        string DecodeFromBase64(string encodedData);
        string EncodeToBase64(string plainText);
        string EncodeToBase64(byte[] textBytes);
    }
}