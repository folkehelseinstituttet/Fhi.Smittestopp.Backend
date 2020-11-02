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

namespace DIGNDB.App.SmitteStop.Testing.JobTests
{
    [TestFixture]
    public class GenerateZipFilesJobTests
    {
        private const string SettingsKey = "ZipFilesLastCreatedDate";
        private readonly DateTime _exampleDatetime = DateTime.Now;

        [Test]
        public void TestGenerateZipFilesWhenZipFilesLastCreatedDateIsNull()
        {
            var configurationMock = new Mock<IConfiguration>();
            var zipFileServiceMock = new Mock<IZipFileService>();
            var settingRepositoryMock = new Mock<ISettingRepository>();
            var dateTimeNowWrapperMock = new Mock<IDateTimeNowWrapper>();
            var loggerMock = new Mock<ILogger<UpdateZipFilesJob>>();
            dateTimeNowWrapperMock.Setup(mock => mock.UtcNow)
                .Returns(_exampleDatetime);

            var generateZipFilesJob = new UpdateZipFilesJob(configurationMock.Object, zipFileServiceMock.Object,
                settingRepositoryMock.Object, loggerMock.Object, dateTimeNowWrapperMock.Object);

            var generateZipFilesAction = new Action(() => generateZipFilesJob.GenerateZipFiles());

            generateZipFilesAction.Should().NotThrow();
            settingRepositoryMock.Verify(mock => mock.FindSettingByKey(SettingsKey),Times.Once);
            zipFileServiceMock.Verify(mock => mock.UpdateZipFiles(DateTime.Today, _exampleDatetime), Times.Once);
            settingRepositoryMock.Verify(mock => mock.SetSetting(SettingsKey, _exampleDatetime.ToString()), Times.Once);
        }

        [Test]
        public void TestGenerateZipFilesWhenZipFilesLastCreatedDateIsLess14Days()
        {
            var configurationMock = new Mock<IConfiguration>();
            var zipFileServiceMock = new Mock<IZipFileService>();
            var settingRepositoryMock = new Mock<ISettingRepository>();
            var loggerMock = new Mock<ILogger<UpdateZipFilesJob>>();
            settingRepositoryMock.Setup(mock => mock.FindSettingByKey(SettingsKey))
                .Returns(new Setting {Value = DateTime.Today.Date.AddDays(-14 + (-1)).ToShortDateString()});
            var dateTimeNowWrapperMock = new Mock<IDateTimeNowWrapper>();
            dateTimeNowWrapperMock.Setup(mock => mock.UtcNow)
                .Returns(_exampleDatetime);

            var generateZipFilesJob = new UpdateZipFilesJob(configurationMock.Object, zipFileServiceMock.Object,
                settingRepositoryMock.Object, loggerMock.Object, dateTimeNowWrapperMock.Object);

            var generateZipFilesAction = new Action(() => generateZipFilesJob.GenerateZipFiles());

            generateZipFilesAction.Should().NotThrow();
            settingRepositoryMock.Verify(mock => mock.FindSettingByKey(SettingsKey),Times.Once);
            zipFileServiceMock.Verify(mock => mock.UpdateZipFiles(DateTime.Today.Date.AddDays(-14), _exampleDatetime), Times.Once);
            settingRepositoryMock.Verify(mock => mock.SetSetting(SettingsKey, _exampleDatetime.ToString()), Times.Once);
        }
    }
}