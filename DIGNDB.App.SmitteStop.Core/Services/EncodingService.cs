using DIGNDB.App.SmitteStop.Core.Contracts;
using System;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class EncodingService : IEncodingService
    {
        public string DecodeFromBase64(string encodedData)
        {
            if (encodedData == null) return null;

            byte[] encodedBytes = Convert.FromBase64String(encodedData);
            return System.Text.Encoding.UTF8.GetString(encodedBytes);
        }

        public string EncodeToBase64(string plainText)
        {
            if (plainText == null) return null;

            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return EncodeToBase64(plainTextBytes);
        }

        public string EncodeToBase64(byte[] textBytes)
        {
            return textBytes == null ? null : Convert.ToBase64String(textBytes);
        }
    }
}