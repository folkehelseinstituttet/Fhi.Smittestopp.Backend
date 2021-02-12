using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Settings;
using DIGNDB.APP.SmitteStop.Jobs.Services;
using FederationGatewayApi.Services.Settings;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class SettingsServiceTest
    {
        public DigNDB_SmittestopContext _dbContext { get; private set; }

        [SetUp]
        public void CreateOptions()
        {
            var dbName = "TEST_DB_" + DateTime.UtcNow;
            var options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>().UseInMemoryDatabase(databaseName: dbName).Options;
            _dbContext = CreateDataBaseContext(options);
        }

        private DigNDB_SmittestopContext CreateDataBaseContext(DbContextOptions<DigNDB_SmittestopContext> options)
        {
            var context = new DigNDB_SmittestopContext(options);
            context.Database.EnsureDeleted();
            return context;
        }

        [Test]
        public void GatewaySyncState_SettingDoesNotExist_ShouldReturnEmptyObjectDto()
        {
            var settingsRepository = new SettingRepository(_dbContext);
            var gatewaySyncSettingsDao = new GatewaySyncStateSettingsDao(settingsRepository);
            var settingService = new SettingsService(gatewaySyncSettingsDao);

            var syncState = settingService.GetGatewayUploadState();

            syncState.Should()
                .NotBeNull();
            syncState.CreationDateOfLastUploadedKey.Should()
                .BeNull();
        }

        [Test]
        public void GatewaySyncState_SettingWasSaveWithoutValue_ShouldReturnEmptyObjectDto()
        {
            var settingsRepository = new SettingRepository(_dbContext);
            var gatewaySyncSettingsDao = new GatewaySyncStateSettingsDao(settingsRepository);
            var settingService = new SettingsService(gatewaySyncSettingsDao);

            var testState = new GatewayUploadState();
            settingService.SaveGatewaySyncState(testState);

            var syncState = settingService.GetGatewayUploadState();
            syncState.Should()
              .NotBeNull();
            syncState.CreationDateOfLastUploadedKey.Should()
                .BeNull();
        }

        [Test]
        public void GatewaySyncState_SettingWasSaveWithoutValue_ShouldReturnSameValue()
        {
            // .: Setup
            var settingsRepository = new SettingRepository(_dbContext);
            var gatewaySyncSettingsDao = new GatewaySyncStateSettingsDao(settingsRepository);
            var settingService = new SettingsService(gatewaySyncSettingsDao);

            var expectedDate = 1601000400;
            var testState = new GatewayUploadState()
            {
                CreationDateOfLastUploadedKey = expectedDate
            };
            // .: Act
            settingService.SaveGatewaySyncState(testState);
            var syncState = settingService.GetGatewayUploadState();

            // .: Validate
            syncState.Should()
              .NotBeNull()
              .And.IsSameOrEqualTo(testState);

            syncState.CreationDateOfLastUploadedKey.Should()
                .Be(expectedDate);        
        }
    }
}
