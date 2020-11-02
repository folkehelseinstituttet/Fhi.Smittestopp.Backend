using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class SettingRepositoryTests
    {
        DbContextOptions<DigNDB_SmittestopContext> _options;

        [SetUp]
        public void CreateOptions()
        {
            var dbName = "TEST_DB_" + DateTime.UtcNow;
            _options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>().UseInMemoryDatabase(databaseName: dbName).Options;
        }

        private IList<Setting> CreateMockedListSettings()
        {
            var data = new List<Setting>();

            data.Add(new Setting
            {
                Key = $"key",
                Value = $"value"
            });
            return data;
        }

        private DigNDB_SmittestopContext CreatePopulatedContext<T>(IEnumerable<T> entities)
            where T : class
        {
            var context = new DigNDB_SmittestopContext(_options);
            context.Database.EnsureDeleted();
            context.AddRange(entities);
            context.SaveChanges();

            return context;
        }

        [Test]
        public void TestGetByKey()
        {
            var entities = CreateMockedListSettings();
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);

            var settingRepository = new SettingRepository(context);

            var insertedEntities = settingRepository.FindSettingByKey("key");

            Assert.AreEqual(insertedEntities, entities.First());
        }

        [Test]
        public void Test()
        {
            var context = new DigNDB_SmittestopContext(_options);
            var entities = CreateMockedListSettings();
            context.Database.EnsureDeleted();
            var settingRepository = new SettingRepository(context);

            var key = "key";
            var value = "value";
            settingRepository.SetSetting(key, value);
            var retrievedEntity = context.Setting.Where(x => x.Key == key).Single();
            var retrievedValue = retrievedEntity.Value;

            var value2 = "value2";
            settingRepository.SetSetting(key, value2);
            var retrievedEntity2 = context.Setting.Where(x => x.Key == key).Single();
            var retrievedValue2 = retrievedEntity2.Value;

            Assert.AreEqual(value, retrievedValue);
            Assert.AreEqual(value2, retrievedValue2);
        }
    }
}
