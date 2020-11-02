using Hangfire;
using System.ComponentModel.DataAnnotations;

namespace DIGNDB.APP.SmitteStop.Jobs.Config
{
    public class PeriodicJobBaseConfig
    {
        private string _cronExpression;


        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public bool RunPeriodically { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CronExpression
        {
            get => RunPeriodically ? _cronExpression : Cron.Never();

            set => _cronExpression = value;
        }
    }
}
