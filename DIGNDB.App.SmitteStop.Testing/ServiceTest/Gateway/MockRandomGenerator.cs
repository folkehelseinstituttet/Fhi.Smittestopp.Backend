using DIGNDB.App.SmitteStop.Domain.Dto;
using System;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class MockRandomGenerator
    {
        public Random _rnd { get; set; }

        public MockRandomGenerator()
        {
            _rnd = new Random();
        }
        public byte[] GenerateKeyData(int byteSize)
        {
            Byte[] b = new Byte[byteSize];
            _rnd.NextBytes(b);
            return b;
        }

        public int GetIntFromInterval(int leftLimit, int rightLimit)
        {
            int rndSize = _rnd.Next(leftLimit, rightLimit);
            return rndSize;

        }

        public string GetReportType()
        {
            var index =  GetIntFromInterval(1,5);
            string reportType = Enum.GetName(typeof(ReportType), index);
            return reportType;
        }
    }
}
