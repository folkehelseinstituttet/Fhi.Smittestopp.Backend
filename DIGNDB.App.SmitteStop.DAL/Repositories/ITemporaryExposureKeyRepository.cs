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

        Task AddUniqueTemporaryExposureKeys(IList<TemporaryExposureKey> temporaryExposureKeys);

        IList<TemporaryExposureKey> GetKeysOnlyFromApiOriginCountry(DateTime uploadedOn, int fetchCommandTimeout);

        int GetCountOfKeysByUpladedDayAndSource(DateTime uploadDate, KeySource gateway);

        void RemoveKeys(List<TemporaryExposureKey> keys);

        void UpdateKeysRollingStartField(List<TemporaryExposureKey> keys);

        IList<TemporaryExposureKey> GetAllKeysNextBatchWithOriginId(int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetAllKeysNextBatch(int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetOriginCountryKeysForPeriodNextBatch(DateTime startDate, int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetAllTemporaryExposureKeysForPeriodNextBatch(DateTime startDate, int numberOfRecordsToSkip, int batchSize);

        Task<IList<TemporaryExposureKey>> GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(long rollingStartNumberThreshold, int numberOfRecordsToSkip, int batchSize);

        IList<TemporaryExposureKey> GetKeysOnlyFromApiOriginCountryUploadedAfterTheDateForGatewayUploadForWhichConsentWasGiven(DateTime uploadedOnAndLater, int numberOfRecordToSkip, int maxCount, KeySource[] sources);
    }
}
