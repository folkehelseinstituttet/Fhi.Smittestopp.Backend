using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface ITemporaryExposureKeyRepository
    {
        Task<IList<TemporaryExposureKey>> GetAll();

        Task<IList<byte[]>> GetAllKeyData();

        Task<TemporaryExposureKey> GetById(Guid id);

        Task AddTemporaryExposureKey(TemporaryExposureKey temporaryExposureKey);

        Task AddTemporaryExposureKeys(IList<TemporaryExposureKey> temporaryExposureKeys);

        IList<TemporaryExposureKey> GetTemporaryExposureKeysWithDkOrigin(DateTime uploadedOn, int fetchCommandTimeout);

        int GetCountOfKeysByUpladedDayAndSource(DateTime uploadDate, KeySource gateway);

        void RemoveKeys(List<TemporaryExposureKey> keys);

        IList<TemporaryExposureKey> GetAllKeysNextBatch(int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetDkTemporaryExposureKeysForPeriodNextBatch(DateTime startDate, int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetAllTemporaryExposureKeysForPeriodNextBatch(DateTime startDate, int numberOfRecordsToSkip, int batchSize);

        Task<IList<TemporaryExposureKey>> GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(long rollingStartNumberThreshold, int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetDkTemporaryExposureKeysUploadedAfterTheDateForGatewayUpload(DateTime uploadedOnAndLater, int numberOfRecordToSkip, int maxCount, KeySource[] sources);
    }
}
