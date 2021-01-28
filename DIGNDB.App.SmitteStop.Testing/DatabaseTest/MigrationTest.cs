using DIGNDB.App.SmitteStop.DAL.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Testing.DatabaseTest
{
    [TestFixture]
    public class MigrationTest
    {
       [Test]
        public void HasNoPendingModelChanges()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DigNDB_SmittestopContext>();
            optionsBuilder
                .UseSqlServer(new SqlConnection()) 
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var databaseMigrationContext = new DigNDB_SmittestopContext(optionsBuilder.Options);

            var modelDiffer = databaseMigrationContext.GetService<IMigrationsModelDiffer>();
            var migrationsAssembly = databaseMigrationContext.GetService<IMigrationsAssembly>();

            var modelDifferences = modelDiffer.GetDifferences(migrationsAssembly.ModelSnapshot.Model, databaseMigrationContext.Model);

            Assert.AreEqual(0, modelDifferences.Count);
        }
        
        [Test]
        public void HasValidTargetModel()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DigNDB_SmittestopContext>();
            optionsBuilder
                .UseSqlServer(new SqlConnection()) 
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var databaseMigrationContext = new DigNDB_SmittestopContext(optionsBuilder.Options);

            var modelDiffer = databaseMigrationContext.GetService<IMigrationsModelDiffer>();
            var migrationsAssembly = databaseMigrationContext.GetService<IMigrationsAssembly>();

            var latestMigration = migrationsAssembly.Migrations.Last();
            var targetModel = migrationsAssembly.CreateMigration(latestMigration.Value, databaseMigrationContext.Database.ProviderName).TargetModel;

            var modelDifferences = modelDiffer.GetDifferences(migrationsAssembly.ModelSnapshot.Model, targetModel);

            Assert.AreEqual(0, modelDifferences.Count);
        } 
    }
}