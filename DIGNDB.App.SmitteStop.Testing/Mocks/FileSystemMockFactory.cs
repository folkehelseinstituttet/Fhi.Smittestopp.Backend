using DIGNDB.App.SmitteStop.Core.Contracts;
using Moq;
using System;
using System.IO;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class FileSystemMockFactory : MockFactory<IFileSystem>
    {
        public FileSystemMockFactory() : base()
        {
            mockedObject.Setup(x => x.GetFileNamesFromDirectory(It.IsAny<string>())).Returns(new string[] { "file1", "file2" });
            mockedObject.Setup(x => x.JoinPaths(It.IsAny<string[]>())).Returns<string[]>(x => Path.Join(x));
            mockedObject.Setup(x => x.DeleteFile(It.IsAny<string>())).Verifiable();
            mockedObject.Setup(x => x.CreateDirectory(It.IsAny<string>())).Verifiable();
            mockedObject.Setup(x => x.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>())).Verifiable();
            mockedObject.Setup(x => x.GetCreationDateUTC(It.IsAny<string>())).Returns(DateTime.UtcNow);
        }
    }
}
