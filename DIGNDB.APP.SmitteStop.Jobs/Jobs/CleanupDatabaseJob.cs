using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class CleanupDatabaseJob : ICleanupDatabaseJob
    {
        private readonly IDatabaseKeysValidationService _databaseKeysValidationService;
        public CleanupDatabaseJob(IDatabaseKeysValidationService databaseKeysValidationService)
        {
            _databaseKeysValidationService = databaseKeysValidationService;
        }

        public void ValidateKeysOnDatabase(int batchSize)
        {
            _databaseKeysValidationService.FindAndRemoveInvalidKeys(batchSize);
        }
    }
}
