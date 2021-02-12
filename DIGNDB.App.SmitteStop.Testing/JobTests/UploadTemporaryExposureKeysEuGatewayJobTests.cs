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
    public class UploadTemporaryExposureKeysEuGatewayJobTests
    {
        private const string SettingsKey = "MaximumNumberOfDaysBack";
        private readonly DateTime _exampleDatetime = DateTime.Now;


        [Test]
        public void TestUpload()
        {
            //Arrange
            var uploadConfig = new UploadKeysToGatewayJobConfig() { BatchSize = 10000, UploadKeysAgeLimitInDays = 7 };
            var euServiceMock = new Mock<IEuGatewayService>();
            var configMock = new Mock<UploadKeysToGatewayJobConfig>();
            var loggerMock = new Mock<ILogger<UploadTemporaryExposureKeysEuGatewayJob>>();
            euServiceMock.Setup(mock => mock.UploadKeysToTheGateway(uploadConfig.UploadKeysAgeLimitInDays, uploadConfig.BatchSize, null)).Verifiable();
            var uploadEuKeysJob = new UploadTemporaryExposureKeysEuGatewayJob(
                uploadConfig,
                euServiceMock.Object,
                loggerMock.Object
            );

            //Act
            var uploadEuKeysJobAction = new Action(() => uploadEuKeysJob.Invoke());

            //Assert
            uploadEuKeysJobAction.Should().NotThrow();
            euServiceMock.Verify(mock => mock.UploadKeysToTheGateway(uploadConfig.UploadKeysAgeLimitInDays, uploadConfig.BatchSize, null), Times.Once);
        }
    }
}