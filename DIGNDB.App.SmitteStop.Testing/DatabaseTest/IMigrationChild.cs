namespace DIGNDB.App.SmitteStop.Testing.DatabaseTest
{
    public interface IMigrationChild
    {
        void BuildTargetModel();
        void Up();
        void Down(); 
    }
}