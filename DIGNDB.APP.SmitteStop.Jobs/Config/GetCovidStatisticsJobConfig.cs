﻿using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class GetCovidStatisticsJobConfig : PeriodicJobBaseConfig
    {
        [Range(0, 23, ErrorMessage = "Please enter correct MakeAlertIfDataIsMissingAfterHour")]
        public int MakeAlertIfDataIsMissingAfterHour { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CovidStatisticsFolder { get; set; }

        public int TestsConductedTotalExtra { get; set; }
    }
}