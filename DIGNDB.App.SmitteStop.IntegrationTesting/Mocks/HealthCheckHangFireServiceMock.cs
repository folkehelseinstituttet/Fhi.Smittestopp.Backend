﻿using System;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.API.Contracts;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class HealthCheckHangFireServiceMock : IHealthCheckHangFireService
    {
        public IMonitoringApi GetHangFireMonitoringApi()
        {
            return new MonitoringApiMock();
        }
    }

    public class MonitoringApiMock : IMonitoringApi
    {
        public IList<QueueWithTopEnqueuedJobsDto> Queues()
        {
            throw new NotImplementedException();
        }

        public IList<ServerDto> Servers()
        {
            return new List<ServerDto> {new ServerDto()};
        }

        public JobDetailsDto JobDetails(string jobId)
        {
            throw new NotImplementedException();
        }

        public StatisticsDto GetStatistics()
        {
            throw new NotImplementedException();
        }

        public JobList<EnqueuedJobDto> EnqueuedJobs(string queue, int @from, int perPage)
        {
            throw new NotImplementedException();
        }

        public JobList<FetchedJobDto> FetchedJobs(string queue, int @from, int perPage)
        {
            throw new NotImplementedException();
        }

        public JobList<ProcessingJobDto> ProcessingJobs(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public JobList<ScheduledJobDto> ScheduledJobs(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public JobList<SucceededJobDto> SucceededJobs(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public JobList<FailedJobDto> FailedJobs(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public JobList<DeletedJobDto> DeletedJobs(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public long ScheduledCount()
        {
            throw new NotImplementedException();
        }

        public long EnqueuedCount(string queue)
        {
            throw new NotImplementedException();
        }

        public long FetchedCount(string queue)
        {
            throw new NotImplementedException();
        }

        public long FailedCount()
        {
            return 0L;
        }

        public long ProcessingCount()
        {
            throw new NotImplementedException();
        }

        public long SucceededListCount()
        {
            throw new NotImplementedException();
        }

        public long DeletedListCount()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, long> SucceededByDatesCount()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, long> FailedByDatesCount()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, long> HourlySucceededJobs()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, long> HourlyFailedJobs()
        {
            throw new NotImplementedException();
        }
    }
}
