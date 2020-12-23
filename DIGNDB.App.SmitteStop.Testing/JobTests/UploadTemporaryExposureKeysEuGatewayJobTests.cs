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