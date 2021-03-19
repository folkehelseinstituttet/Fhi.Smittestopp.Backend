using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.EuGateway;
using DIGNDB.APP.SmitteStop.Jobs.Jobs;
using Hangfire;

namespace DIGNDB.APP.SmitteStop.Jobs
{
    public class ScheduledJobsConfiguration
    {
        private const string TestJobPrefix = "[Test]";

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

            var getCovidStatisticsConfig = jobsConfig.GetCovidStatistics;
            RecurringJob.AddOrUpdate<GetCovidStatisticsJob>(recurringJobId: getCovidStatisticsConfig.Name, methodCall: job => job.ObtainCovidStatistics(), getCovidStatisticsConfig.CronExpression);

            // This job entry should be removed after the problem with rolingStart, that is causing keys to be rejected by Google Exposure Notifcation, have been resolved.
            RecurringJob.AddOrUpdate<CleanupDatabaseJob>(recurringJobId: "maintenance-rollingStart-check-on-datbase-keys", methodCall: job => job.ValidateRollingStartOnDatabaseKeys(1000), Cron.Never);

        }
    }
}
