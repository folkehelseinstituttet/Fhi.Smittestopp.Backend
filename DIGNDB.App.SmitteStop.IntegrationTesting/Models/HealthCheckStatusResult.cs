using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Models
{
    public class HealthCheckStatusResult
    {
        public string status { get; set; }
        public Results results { get; set; }
    }

    public class HealthException
    {
        public string message { get; set; }
        public string stackTrace { get; set; }
    }

    public class HangFire
    {
        public string status { get; set; }
        public string description { get; set; }
        public Exception exception { get; set; }
    }

    public class LogFiles
    {
        public string status { get; set; }
        public string description { get; set; }
        public HealthException healthException { get; set; }
    }

    //public class Data
    //{
    //    public string State { get; set; }
    //}

    public class Results
    {
        public LogFiles LogFiles { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
