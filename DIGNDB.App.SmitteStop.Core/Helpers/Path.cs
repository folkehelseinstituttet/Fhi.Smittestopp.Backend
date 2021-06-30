using DIGNDB.App.SmitteStop.Core.Contracts;
using System.IO;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class PathHelper : IPathHelper
    {
        public string GetDirectoryName(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            return directoryName;
        }
    }
}
