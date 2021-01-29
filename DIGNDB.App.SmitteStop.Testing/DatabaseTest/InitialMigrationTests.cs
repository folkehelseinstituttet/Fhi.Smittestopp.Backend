using DIGNDB.App.SmitteStop.DAL.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.DatabaseTest
{
    public class InitialMigrationChild : InitialMigration, IMigrationChild
    {
        public void Up()
        {
            base.Up(new MigrationBuilder(string.Empty));
        }
        public void Down()
        {
            base.Down(new MigrationBuilder(string.Empty));
        }

        public void BuildTargetModel()
        {
            base.BuildTargetModel(new ModelBuilder(new ConventionSet()));
        }
    }
    
    [TestFixture]
    public class InitialMigrationTests : GenericMigrationTests<InitialMigrationChild>
    {
        
        [Test]
        public new void TestUpDown()
        {
            base.TestUpDown();
        }

        [Test]
        public new void TestBuildTargetModel()
        {
            base.TestBuildTargetModel();
        }
    }
}