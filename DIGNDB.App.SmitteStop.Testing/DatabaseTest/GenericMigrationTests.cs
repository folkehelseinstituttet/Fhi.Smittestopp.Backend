using FluentAssertions;
using System;

namespace DIGNDB.App.SmitteStop.Testing.DatabaseTest
{
    public class GenericMigrationTests<TMigration> where TMigration : IMigrationChild, new()
    {
        protected void TestUpDown()
        {
            var initialMigrationChild = new TMigration();

            var executeUpAction = new Action(() => initialMigrationChild.Up());
            executeUpAction.Should().NotThrow();
            
            var executeDownAction = new Action(() => initialMigrationChild.Down());
            executeDownAction.Should().NotThrow();
        }

        protected void TestBuildTargetModel()
        {
            var initialMigrationChild = new TMigration();
            
            var executeBuildTargetModel = new Action(() => initialMigrationChild.BuildTargetModel());
            executeBuildTargetModel.Should().NotThrow();
        }
    }
}