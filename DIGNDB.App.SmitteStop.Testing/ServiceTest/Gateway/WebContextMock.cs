using System.IO;
using System.Net;
using System.Net.Http;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest.Gateway
{
    public class WebContextMock
    {

        public MockRandomGenerator _rndGenerator { get; set; }
        // private const string ResponseBodySource = @"./../../../../../ServiceTest/Gateway/Files/GatewayResponse.txt";
        private const string ResponseBodySource = @"./ServiceTest/Gateway/Files/GatewayResponse.txt";

        public WebContextMock()
        {
            _rndGenerator = new MockRandomGenerator();
        }

        public string MockValidBodyJSON()
        {
            var response = File.ReadAllText(ResponseBodySource); 
            return response;
        }
        public string MockInvalidBodyJSON()
        {
            return "#!blahbidy blah";
        }


        public HttpResponseMessage MockHttpResponse(bool empty = false)
        {
            var content = MockValidBodyJSON();
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK

            };

            if(empty == true) 
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
