using DIGNDB.App.SmitteStop.Core.Helpers;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.App.SmitteStop.API.HealthChecks
{
    /// <summary>
    /// Super class for health checks that to be configured to run after a specific hour
    /// </summary>
    public class TimedHealthCheck
    {
        /// <summary>
        /// Decides whether now is before the hour passed as argument
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="logger"></param>
        /// <returns>True if now is before the passed hour</returns>
        public bool TooEarly(int hour, ILogger logger)
        {
            try
            {
                var callAfter = DateTime.Now.Date;
                callAfter = callAfter.ChangeTime(hour);
                var now = DateTime.Now;
                if (now < callAfter)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message} - {e.StackTrace}");
                throw;
            }

            return false;
        }
    }
}
