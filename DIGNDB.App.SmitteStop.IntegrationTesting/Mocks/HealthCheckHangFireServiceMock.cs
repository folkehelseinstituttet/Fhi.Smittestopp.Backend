using DIGNDB.App.SmitteStop.API.Contracts;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.IntegrationTesting.Mocks
{
    public class HealthCheckHangFireServiceMock : IHealthCheckHangFireService
    {
        private long FailedCountOverwrite { get; }
        private int ServersCountOverWrite { get; }

        public HealthCheckHangFireServiceMock(long failedCount, int serversCount)
        {
            FailedCountOverwrite = failedCount;
            ServersCountOverWrite = serversCount;
        }

        public IMonitoringApi GetHangFireMonitoringApi()
        {
            var monitoringApiMock = new MonitoringApiMock(FailedCountOverwrite, ServersCountOverWrite);
            return monitoringApiMock;
        }
    }

    public class MonitoringApiMock : IMonitoringApi
    {
        private long FailedCountOverwrite { get; }
        private int ServersCountOverWrite { get; }

        public MonitoringApiMock(long failedCount, int serversCount)
        {
            FailedCountOverwrite = failedCount;
            ServersCountOverWrite = serversCount;
        }

        public IList<QueueWithTopEnqueuedJobsDto> Queues()
        {
            throw new NotImplementedException();
        }

        public IList<ServerDto> Servers()
        {
            var servers = new List<ServerDto>();
            for (var i = 0; i < ServersCountOverWrite; i++)
            {
                servers.Add(new ServerDto());
            }

            return servers;
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
            return FailedCountOverwrite;
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
