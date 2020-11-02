using Moq;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class MockFactory<T> where T : class
    {
        protected Mock<T> mockedObject;

        public MockFactory()
        {
            this.mockedObject = new Mock<T>(MockBehavior.Strict);
        }

        public Mock<T> GetMock()
        {
            return mockedObject;
        }
    }
}
