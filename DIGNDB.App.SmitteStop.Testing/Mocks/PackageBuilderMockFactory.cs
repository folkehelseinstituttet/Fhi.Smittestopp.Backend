using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using Moq;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class PackageBuilderMockFactory : MockFactory<IPackageBuilderService>
    {
        public PackageBuilderMockFactory() : base()
        {
            mockedObject.Setup(x => x.BuildPackageContentV2(It.IsAny<DateTime>(), It.IsAny<ZipFileOrigin>()))
                .Returns(new List<byte[]>() { new byte[] { 1, 2, 3, 4 }, { new byte[] { 1, 2, 3, 4 } } });
        }
    }
}
