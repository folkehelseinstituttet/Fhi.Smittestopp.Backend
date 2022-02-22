using System;
using System.Text.Json;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class ImportantInfoRequest
    {
        public string lang { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}