using DIGNDB.App.SmitteStop.Core.Contracts;
using Moq;
using System;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class ZipFileInfoServiceMockFactory : MockFactory<IZipFileInfoService>
    {
        public ZipFileInfoServiceMockFactory() : base()
        {
            mockedObject.Setup(x => x.GetNextBatchNumberForGivenDay(It.IsAny<string[]>(), It.IsAny<DateTime>())).Returns(1);

        }
    }
}
