﻿using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class HealthCheckHangFireService : IHealthCheckHangFireService
    {
        private readonly HangFireConfig _hangFireConfiguration;

        private IMonitoringApi _hangFireMonitoringApi;
        private static JobStorage _jobStorageCurrent;

        public HealthCheckHangFireService(HangFireConfig hangFireConfiguration)
        {
            _hangFireConfiguration = hangFireConfiguration;
        }

        public IMonitoringApi GetHangFireMonitoringApi()
        {
            if (_jobStorageCurrent == null)
            {
                InitializeHangFire();
            }

            if (_jobStorageCurrent != null)
            {
                _hangFireMonitoringApi = _jobStorageCurrent.GetMonitoringApi();
            }

            return _hangFireMonitoringApi;
        }

        public List<RecurringJobDto> GetRecurringJobs()
        {
            var recurringJobs = _jobStorageCurrent.GetConnection().GetRecurringJobs();
            return recurringJobs;
        }

        private void InitializeHangFire()
        {
            JobStorage.Current = new SqlServerStorage(_hangFireConfiguration.HangFireConnectionString);
            _jobStorageCurrent = JobStorage.Current;
        }
    }
}
