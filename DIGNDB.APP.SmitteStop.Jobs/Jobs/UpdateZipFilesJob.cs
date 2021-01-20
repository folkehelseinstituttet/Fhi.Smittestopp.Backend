﻿using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.APP.SmitteStop.Jobs.Jobs.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.Jobs
{
    public class UpdateZipFilesJob : IUpdateZipFilesJob
    {
        private readonly IZipFileService _zipFileService;
        private readonly ISettingRepository _settingRepository;
        private readonly string lastCreatedDateSettingKey = "ZipFilesLastCreatedDate";
        private readonly IDateTimeNowWrapper _dateTimeNowWrapper;
        private readonly ILogger<UpdateZipFilesJob> _logger;

        public UpdateZipFilesJob(IZipFileService zipFileService, ISettingRepository settingRepository, ILogger<UpdateZipFilesJob> logger, IDateTimeNowWrapper dateTimeNowWrapper)
        {
            _zipFileService = zipFileService;
            _settingRepository = settingRepository;
            _logger = logger;
            _dateTimeNowWrapper = dateTimeNowWrapper;
        }

        public void GenerateZipFiles()
        {
            try
            {
                var currentDate = _dateTimeNowWrapper.UtcNow;
                DateTime lastCreationDate = GetLastCreationDateFromSettings(currentDate);
                _zipFileService.UpdateZipFiles(lastCreationDate, currentDate);
                _settingRepository.SetSetting(lastCreatedDateSettingKey, currentDate.ToString());
                _logger.LogInformation("Updated zip files");
            }
            catch (Exception e)
            {
                var innerException =
                    e.InnerException != null
                        ? e.InnerException.Message + " : " + e.InnerException.StackTrace + " : "
                        : string.Empty;

                _logger.LogError($"Updating of zip files failed. {innerException}{e.Message}");
                throw;
            }
        }

        private DateTime GetLastCreationDateFromSettings(DateTime currentDate)
        {
            var lastCreatedDateSetting = _settingRepository.FindSettingByKey(lastCreatedDateSettingKey);
            var _14DaysAgo = currentDate.Date.AddDays(-14);
            DateTime lastCreatedDate;
            if (lastCreatedDateSetting == null || !DateTime.TryParse(lastCreatedDateSetting.Value, out lastCreatedDate))
            {
                lastCreatedDate = currentDate.Date;
            }
            else
            {
                if (lastCreatedDate < _14DaysAgo)
                {
                    lastCreatedDate = _14DaysAgo;
                }
            }
            return lastCreatedDate;
        }
    }
}
