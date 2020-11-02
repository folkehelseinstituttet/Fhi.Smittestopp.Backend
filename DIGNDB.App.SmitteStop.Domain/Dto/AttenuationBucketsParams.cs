using System;
using System.Collections.Generic;
using System.Text;

namespace DIGNDB.App.SmitteStop.Core.Models
{
    public class AttenuationBucketsParams
    {
        public double ExposureTimeThreshold { get; set; }
        public double LowAttenuationBucketMultiplier { get; set; }
        public double MiddleAttenuationBucketMultiplier { get; set; }
        public double HighAttenuationBucketMultiplier { get; set; }
    }
}
