﻿namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IDatabaseKeysValidationService
    {
        void FindAndRemoveInvalidKeys(int batchSize);
        void FindInvalidRollingStartEntrysAndUpdateEntry(int batchSize);

    }
}
