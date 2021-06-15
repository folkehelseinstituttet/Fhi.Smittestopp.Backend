using System.IO;
using System.Net;
using System.Net.Http;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class WebContextMock
    {
        private const string ResponseBodySource = @"./ServiceTest/Gateway/Files/GatewayResponse.txt";

        public string MockValidBodyJson()
        {
            var response = File.ReadAllText(ResponseBodySource); 
            return response;
        }

        public string MockNoBatchesBodyJson()
        {
            return "{\"message\": \"Could not find any batches for given date\"}"; ;
        }

        public string MockInvalidBodyJson()
        {
            return "#!blahbidy blah";
        }

        public HttpResponseMessage MockHttpResponse(bool empty = false)
        {
            var content = MockValidBodyJson();
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK

            };

            if(empty) 
            {
                fakeResponse.Content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");
            }
            else
            {
                fakeResponse.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                fakeResponse.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "requestUri");
            }

            return fakeResponse;
        }
    }
}
