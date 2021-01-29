using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIGNDB.App.SmitteStop.Testing.RepositoriesTest
{
    [TestFixture]
    public class GenericRepositoryTests
    {
        DbContextOptions<DigNDB_SmittestopContext> _options;

        [SetUp]
        public void CreateOptions()
        {
            var dbName = "TEST_DB_" + DateTime.UtcNow;
            _options = new DbContextOptionsBuilder<DigNDB_SmittestopContext>().UseInMemoryDatabase(databaseName: dbName).Options;
        }

        private IList<TemporaryExposureKey> CreateMockedListExposureKeys(DateTime expectDate, int numberOfKeys)
        {
            var data = new List<TemporaryExposureKey>();

            for (var i = 0; i < numberOfKeys; i++)
            {
                data.Add(new TemporaryExposureKey
                {
                    CreatedOn = expectDate.Date,
                    Id = Guid.NewGuid(),
                    KeyData = Encoding.ASCII.GetBytes("keyData" + (i + 1)),
                    TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
                });
            }

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
        public void TestGetAll()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);

            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var insertedEntities = genericRepository.GetAll();

            CollectionAssert.AreEquivalent(insertedEntities, entities);
        }

        [Test]
        public void TestGetAllAsync()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);

            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var insertedEntities = genericRepository.GetAllAsync().Result;

            CollectionAssert.AreEquivalent(insertedEntities, entities);
        }

        [Test]
        public void TestGetById()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);
            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var firstEntity = entities.First();
            var firstEntityId = firstEntity.Id;

            var insertedEntity = genericRepository.GetById(firstEntityId);

            Assert.AreEqual(insertedEntity, firstEntity);
        }

        [Test]
        public void TestGetByProperty()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);
            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var firstEntity = entities.First();

            var insertedEntity =
                genericRepository
                    .Get(e => e.Id == firstEntity.Id, es => es.OrderBy(e => e.Id))
                    .Single();;

            Assert.AreEqual(insertedEntity, firstEntity);
        }

        [Test]
        public void TestDelete()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);
            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var firstEntity = entities.First();

            context.Entry(firstEntity).State = EntityState.Detached;
            context.SaveChanges();
            genericRepository.Delete(firstEntity);
            context.SaveChanges();

            var retrievedEntity = genericRepository.GetById(firstEntity.Id);

            Assert.AreEqual(retrievedEntity, null);
        }

        [Test]
        public void TestDeleteById()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);
            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var firstEntity = entities.First();

            genericRepository.Delete(firstEntity.Id);
            context.SaveChanges();

            var retrievedEntity = genericRepository.GetById(firstEntity.Id);

            Assert.AreEqual(retrievedEntity, null);
        }

        [Test]
        public void TestSave()
        {
            var context = new DigNDB_SmittestopContext(_options);
            context.Database.EnsureDeleted();

            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var entity = new TemporaryExposureKey
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                KeyData = new byte[]{1, 2, 3},
                RollingPeriod = 123,
                RollingStartNumber = 123,
                TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
            };

            context.TemporaryExposureKey.Add(entity);

            genericRepository.Save();

            var retrievedEntity = context.TemporaryExposureKey.Find(entity.Id);

            Assert.AreEqual(retrievedEntity, entity);
        }

        [Test]
        public void TestSaveAsync()
        {
            var context = new DigNDB_SmittestopContext(_options);
            context.Database.EnsureDeleted();

            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var entity = new TemporaryExposureKey
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                KeyData = new byte[]{1, 2, 3},
                RollingPeriod = 123,
                RollingStartNumber = 123,
                TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
            };

            context.TemporaryExposureKey.Add(entity);

            genericRepository.SaveAsync();

            var retrievedEntity = context.TemporaryExposureKey.Find(entity.Id);

            Assert.AreEqual(retrievedEntity, entity);
        }

        [Test]
        public void TestInsert()
        {
            var context = new DigNDB_SmittestopContext(_options);
            context.Database.EnsureDeleted();

            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var entity = new TemporaryExposureKey
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                KeyData = new byte[]{1, 2, 3},
                RollingPeriod = 123,
                RollingStartNumber = 123,
                TransmissionRiskLevel = RiskLevel.RISK_LEVEL_LOW
            };

            genericRepository.Insert(entity);
            context.SaveChanges();

            var retrievedEntity = context.TemporaryExposureKey.Find(entity.Id);

            Assert.AreEqual(retrievedEntity, entity);
        }

        [Test]
        public void TestEdit()
        {
            var expectDate = DateTime.UtcNow;
            var entities = CreateMockedListExposureKeys(expectDate, 2);
            using DigNDB_SmittestopContext context = CreatePopulatedContext(entities);
            var genericRepository = new GenericRepository<TemporaryExposureKey>(context);

            var entity = entities.First();
            entity.RollingPeriod = 567;

            genericRepository.Edit(entity);
            context.SaveChanges();

            var retrievedEntity = context.TemporaryExposureKey.Find(entity.Id);

            Assert.AreEqual(retrievedEntity.RollingPeriod, 567);
        }
    }
}
