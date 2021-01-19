using System;
using System.Globalization;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.APP.SmitteStop.Jobs.Jobs;
using FluentAssertions;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Extensions.Logging;
using DIGNDB.APP.SmitteStop.Jobs.EuGateway;
using FederationGatewayApi.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Config;

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