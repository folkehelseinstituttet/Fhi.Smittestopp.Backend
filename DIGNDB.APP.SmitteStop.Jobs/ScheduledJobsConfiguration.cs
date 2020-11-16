using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Jobs;
using FederationGatewayApi.Services;
using Hangfire;
using System;
using System.Text.Json;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public class ScheduledJobsConfiguration
    {
        const string TestJobPrefix = "[Test]";

        public static void ConfigureScheduledJobs(HangfireConfig hangfireConfig)
        {
            var jobsConfig = hangfireConfig.Jobs;
            var ValidateKeysOnDatabaseConfig = jobsConfig.ValidateKeysOnDatabase;
            RecurringJob.AddOrUpdate<CleanupDatabaseJob>(recurringJobId: ValidateKeysOnDatabaseConfig.Name, methodCall: job => job.ValidateKeysOnDatabase(ValidateKeysOnDatabaseConfig.BatchSize), ValidateKeysOnDatabaseConfig.CronExpression);

            var updateZipConfig = jobsConfig.UpdateZipFiles;
            RecurringJob.AddOrUpdate<UpdateZipFilesJob>(recurringJobId: updateZipConfig.Name, methodCall: job => job.GenerateZipFiles(), updateZipConfig.CronExpression);

            var removeOldZipFilesConfig = jobsConfig.RemoveOldZipFiles;
            RecurringJob.AddOrUpdate<RemoveOldZipFilesJob>(recurringJobId: removeOldZipFilesConfig.Name, methodCall: job => job.RemoveOldZipFiles(hangfireConfig), removeOldZipFilesConfig.CronExpression);
        }
    }
}
