using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions
{
    [Serializable]
    public class CovidStatisticsFileMissingOnServerException : Exception
    {
        public const string DataMissingInfoMessage = "Data is not yet available for covid statistics.";

        public const string DataMissingErrorMessage = DataMissingInfoMessage + " Time threshold for obtaining data passed. Check if data is indeed unavailable";

        public CovidStatisticsFileMissingOnServerException() : base(DataMissingErrorMessage)
        {
        }
    }
}