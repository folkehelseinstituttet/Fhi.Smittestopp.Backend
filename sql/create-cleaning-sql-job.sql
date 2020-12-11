-- Create SQL Stored Procedure, which deletes keys older than 14 days.
-- Procedure works in batches of 1000 keys in order not to lock the table too much.
-- ================================================================================================ --
USE [DigNDB_Smittestop]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR
ALTER PROCEDURE [dbo].[RemoveKeysOlderThan14Days]
AS
    WHILE EXISTS(SELECT TOP (1) ID
                 FROM TemporaryExposureKey with (nolock)
                 WHERE [CreatedOn] < DATEADD(day, -14, GETDATE()))
        BEGIN
            DELETE TemporaryExposureKey
            where ID in (SELECT TOP (1000) [ID]
                         FROM [DigNDB_Smittestop].[dbo].[TemporaryExposureKey] with (nolock)
                         where [CreatedOn] < DATEADD(day, -14, GETDATE()))
        END
GO
-- ================================================================================================ --


-- Create an SQL Job, which calls the procedure above.
-- Job is set up to run every day.
-- ================================================================================================ --
USE [msdb]
GO

BEGIN TRANSACTION
    DECLARE @ReturnCode INT
    SELECT @ReturnCode = 0
    IF NOT EXISTS(
            SELECT name FROM msdb.dbo.syscategories WHERE name = N'[Uncategorized (Local)]' AND category_class = 1)
        BEGIN
            EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
            IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

        END

    DECLARE @jobId BINARY(16)
    EXEC @ReturnCode = msdb.dbo.sp_add_job @job_name=N'temp_exp_key_cleanup',
                       @enabled=1,
                       @notify_level_eventlog=0,
                       @notify_level_email=0,
                       @notify_level_netsend=0,
                       @notify_level_page=0,
                       @delete_level=0,
                       @category_name=N'[Uncategorized (Local)]',
                       @owner_login_name=N'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX', -- REPLACE WITH YOUR LOGIN!
        @job_id = @jobId OUTPUT
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'delete keys older than 14 days',
                       @step_id=1,
                       @cmdexec_success_code=0,
                       @on_success_action=1,
                       @on_success_step_id=0,
                       @on_fail_action=2,
                       @on_fail_step_id=0,
                       @retry_attempts=0,
                       @retry_interval=0,
                       @os_run_priority=0, @subsystem=N'TSQL',
                       @command=N'EXEC [dbo].[RemoveKeysOlderThan14Days]',
                       @database_name=N'DigNDB_Smittestop',
                       @flags=0
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily',
                       @enabled=1,
                       @freq_type=4,
                       @freq_interval=1,
                       @freq_subday_type=1,
                       @freq_subday_interval=0,
                       @freq_relative_interval=0,
                       @freq_recurrence_factor=0,
                       @active_start_date=20200715,
                       @active_end_date=99991231,
                       @active_start_time=10000,
                       @active_end_time=235959,
                       @schedule_uid=N'c21f28c6-cff2-4361-b9d0-e326134437da'
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO
-- ================================================================================================ --


