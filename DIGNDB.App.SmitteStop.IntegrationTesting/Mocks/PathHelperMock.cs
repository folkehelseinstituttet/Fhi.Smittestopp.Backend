using DIGNDB.App.SmitteStop.Core.Contracts;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class PathHelperMock : IPathHelper
    {
        public string GetDirectoryName(string path)
        {
            return path;
        }
    }
}
