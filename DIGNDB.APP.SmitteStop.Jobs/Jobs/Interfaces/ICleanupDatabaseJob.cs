namespace DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces
{
    public interface ICleanupDatabaseJob
    {
        void ValidateKeysOnDatabase(int batchSize);
        void ValidateRollingStartOnDatabaseKeys(int batchSize);
    }
}
