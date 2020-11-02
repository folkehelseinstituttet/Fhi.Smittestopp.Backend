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

            var uploadConfig = jobsConfig.UploadKeysToTheGateway;
            RecurringJob.AddOrUpdate<UploadTemporaryExposureKeysUeGatewayJob>(recurringJobId: uploadConfig.Name, methodCall: job => job.Invoke(), uploadConfig.CronExpression);

            var downloadConfig = jobsConfig.DownloadKeysFromTheGateway;
            RecurringJob.AddOrUpdate<EuGatewayService>(recurringJobId: downloadConfig.Name, methodCall: job => job.DownloadKeysFromGateway(downloadConfig.MaximumNumberOfDaysBack), downloadConfig.CronExpression);

            var removeOldZipFilesConfig = jobsConfig.RemoveOldZipFiles;
            RecurringJob.AddOrUpdate<RemoveOldZipFilesJob>(recurringJobId: removeOldZipFilesConfig.Name, methodCall: job => job.RemoveOldZipFiles(hangfireConfig), removeOldZipFilesConfig.CronExpression);


            var maitenanceCheckConfig = jobsConfig.DailyMaintenanceCheck;

            // workaround for passing arguments https://docs.hangfire.io/en/latest/background-methods/passing-arguments.html
            RecurringJob.AddOrUpdate<MaintenanceDailySummaryJob>(recurringJobId: maitenanceCheckConfig.Name,
                methodCall: job => job.Invoke(MaintenanceDailySummaryJob.SerializeConfig(maitenanceCheckConfig)),
                maitenanceCheckConfig.CronExpression);

            SetupTestJobs(jobsConfig);
        }

        private static void SetupTestJobs(JobsConfig jobsConfig)
        {
            var uploadConfig = jobsConfig.UploadKeysToTheGateway;
            AddTestUpload(uploadConfig, uploadFromLastNumberOfDays: 14, uploadBatchSize: 1, uploadBatchLimit: 1);
            AddTestUpload(uploadConfig, uploadFromLastNumberOfDays: 14, uploadBatchSize: 2, uploadBatchLimit: 1);
            AddTestUpload(uploadConfig, uploadFromLastNumberOfDays: 14, uploadBatchSize: 10, uploadBatchLimit: 4);
        }

        private static void AddTestUpload(UploadKeysToGatewayJobConfig config, int uploadFromLastNumberOfDays, int uploadBatchSize, int uploadBatchLimit)
        {
            var jobName = $"{TestJobPrefix}_{config.Name} FromLastNumberOfDays:{uploadFromLastNumberOfDays} BatchSize:{uploadBatchSize}, BatchLimit:{uploadBatchLimit}";
            RecurringJob.AddOrUpdate<UploadTemporaryExposureKeysUeGatewayTestJob>(recurringJobId: jobName, methodCall: job => job.Invoke(uploadFromLastNumberOfDays, uploadBatchSize, uploadBatchLimit), Cron.Never);
        }
    }
}
