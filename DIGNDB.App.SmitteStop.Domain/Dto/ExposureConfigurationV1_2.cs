using System;
using System.Collections.Generic;
using System.Text;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Models
{
    public class ExposureConfigurationV1_2
    {
        public ExposureConfiguration Configuration { get; set; }
        public AttenuationBucketsParams AttenuationBucketsParams { get; set; }
    }
}
