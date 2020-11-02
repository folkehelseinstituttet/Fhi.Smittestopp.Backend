using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Testing.Mocks;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Jobs;
using DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.JobTests
{
    [TestFixture]
    public class RemoveOldFilesJobTests
    {
        private HangfireConfig _hangfireConfig;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IZipFileInfoService> _zipFileInfo;

        [SetUp]
        public void Init()
        {
            _hangfireConfig = new HangfireConfig();
            _hangfireConfig.ZipFilesFolder = "";
            _hangfireConfig.DaysToInvalidateZipFile = 1;
            _fileSystem = new FileSystemMockFactory().GetMock();
            _zipFileInfo = new ZipFileInfoServiceMockFactory().GetMock();
        }

        [Test]
        public void RemoveOldZipFilesTestShouldRemoveFiles()
        {
            var filename = "test";
            _zipFileInfo.Setup(x => x.CreateZipFileInfoFromPackageName(It.IsAny<string>())).Returns(new ZipFileInfo() { PackageDate = DateTime.UtcNow.AddDays(-2) });
            IRemoveOldZipFilesJob removeOldZipFilesJob = new RemoveOldZipFilesJob(_fileSystem.Object, _zipFileInfo.Object);
            removeOldZipFilesJob.RemoveOldZipFiles(_hangfireConfig);
            _fileSystem.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public void RemoveOldZipFilesTestShouldNotRemoveFiles()
        {
            _zipFileInfo.Setup(x => x.CreateZipFileInfoFromPackageName(It.IsAny<string>())).Returns(new ZipFileInfo() { PackageDate = DateTime.UtcNow });
            IRemoveOldZipFilesJob removeOldZipFilesJob = new RemoveOldZipFilesJob(_fileSystem.Object, _zipFileInfo.Object);
            removeOldZipFilesJob.RemoveOldZipFiles(_hangfireConfig);
            _fileSystem.Verify(x => x.DeleteFile(It.IsAny<string>()), Times.Never);
        }
    }
}
