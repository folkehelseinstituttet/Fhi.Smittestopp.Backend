using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions
{
    [Serializable]
    internal class CovidStatisticsParsingException : Exception
    {
        public const string ExceptionMessage =
            "There was an error while parsing CSV file while trying to collect covid statistics data.";
        public CovidStatisticsParsingException(Exception innerException)
            : base(ExceptionMessage, innerException)
        {

        }
    }
}