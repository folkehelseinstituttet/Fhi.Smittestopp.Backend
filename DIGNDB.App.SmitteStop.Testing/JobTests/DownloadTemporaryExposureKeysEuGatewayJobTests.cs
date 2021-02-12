using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.EuGateway;
using FederationGatewayApi.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.JobTests
{
    [TestFixture]
    public class DownloadTemporaryExposureKeysEuGatewayJobTests
    {
        private const string SettingsKey = "ZipFilesLastCreatedDate";
        private readonly DateTime _exampleDatetime = DateTime.Now;

        [Test]
        public void TestDownload()
        {
            //Arrange
            var downloadConfig = new DownloadKeysFromGatewayJobConfig() { MaximumNumberOfDaysBack = 14  };
            var euServiceMock = new Mock<IEuGatewayService>();
            var loggerMock = new Mock<ILogger<DownloadTemporaryExposureKeysEuGatewayJob>>();
            euServiceMock.Setup(mock => mock.DownloadKeysFromGateway(downloadConfig.MaximumNumberOfDaysBack)).Verifiable();
            var downloadEuKeysJob = new DownloadTemporaryExposureKeysEuGatewayJob(
                downloadConfig,
                euServiceMock.Object,
                loggerMock.Object
            );

            //Act
            var downloadEuKeysAction = new Action(() => downloadEuKeysJob.Invoke());

            //Assert
            downloadEuKeysAction.Should().NotThrow();
            euServiceMock.Verify(mock => mock.DownloadKeysFromGateway(downloadConfig.MaximumNumberOfDaysBack), Times.Once);
        }
    }
}