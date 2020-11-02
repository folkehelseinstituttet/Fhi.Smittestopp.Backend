using System.Net;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class AppleResponseDto
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string Content { get; set; }
    }
}
