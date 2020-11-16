using System.Collections.Generic;
using System.Linq;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DIGNDB.APP.SmitteStop.Jobs.Services
{
    public class DatabaseKeysValidationService : IDatabaseKeysValidationService
    {
        private readonly ITemporaryExposureKeyRepository _repository;
        private readonly IKeyValidator _keyValidator;
        private readonly ILogger _logger;
        public DatabaseKeysValidationService(ITemporaryExposureKeyRepository repository, IKeyValidator keyValidator, ILogger<DatabaseKeysValidationService> logger)
        {
            _repository = repository;
            _keyValidator = keyValidator;
            _logger = logger;
        }

        public void FindAndRemoveInvalidKeys(int batchSize)
        {
            int numberOfRecordsToSkip = 0;
            List<TemporaryExposureKey> databaseKeys;
            List<TemporaryExposureKey> wrongKeys = new List<TemporaryExposureKey>();
            var errorMessage = string.Empty;
            do
            {
                databaseKeys = _repository.GetAllKeysNextBatch(numberOfRecordsToSkip, batchSize).ToList();
                foreach (var key in databaseKeys)
                {
                    if (!_keyValidator.ValidateKeyAPI(key, out errorMessage))
                    {
                        wrongKeys.Add(key);
                    }
                }
                numberOfRecordsToSkip += databaseKeys.Count;
            }
            while (databaseKeys.Count > 0);

            _repository.RemoveKeys(wrongKeys);
            _logger.LogInformation($"Removed {wrongKeys.Count} invalid keys from the database: {JsonConvert.SerializeObject(wrongKeys)}");
        }
    }
}
