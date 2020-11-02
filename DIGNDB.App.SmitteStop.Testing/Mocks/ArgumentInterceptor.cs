using Moq;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    /// <summary>
    /// Helper class for Moq. It is capturing parameter passed to the mocked method for each method call.
    /// 
    /// Sample usage:
    /// var requestArgInterceptor = new ArgumentInterceptor<HttpContent>();
    /// gatewayHttpClientMock.Verify(c => c.PostAsync(It.IsAny<string>(), requestArgInterceptor.CreateCaptureAction()));
    /// </summary>
    /// <typeparam name="T">Type of the captured parameter</typeparam>
    public class ArgumentInterceptor<T>
    {
        private readonly IList<T> _calls = new List<T>();

        public T CreateCaptureAction()
        {
            return It.Is<T>(t => SaveValue(t));
        }

        private bool SaveValue(T t)
        {
            _calls.Add(t);
            return true;
        }
        public IList<T> Calls => _calls;
    }
}
