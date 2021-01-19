using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.Jobs;
using DIGNDB.APP.SmitteStop.Jobs.EuGateway;
using FederationGatewayApi.Services;
using Hangfire;

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

            var uploadConfig = jobsConfig.UploadKeysToTheGateway;
            RecurringJob.AddOrUpdate<UploadTemporaryExposureKeysEuGatewayJob>(recurringJobId: uploadConfig.Name, methodCall: job => job.Invoke(), uploadConfig.CronExpression);

            var downloadConfig = jobsConfig.DownloadKeysFromTheGateway;
            RecurringJob.AddOrUpdate<DownloadTemporaryExposureKeysEuGatewayJob>(recurringJobId: downloadConfig.Name, methodCall: job => job.Invoke(), downloadConfig.CronExpression);

            var removeOldZipFilesConfig = jobsConfig.RemoveOldZipFiles;
            RecurringJob.AddOrUpdate<RemoveOldZipFilesJob>(recurringJobId: removeOldZipFilesConfig.Name, methodCall: job => job.RemoveOldZipFiles(hangfireConfig), removeOldZipFilesConfig.CronExpression);
        }
    }
}
