using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Exceptions
{
    [Serializable]
    internal class CovidStatisticsStreamProcessingException : Exception
    {
        private const string ExceptionMessage =
            "There was an error when processing a stream while building covid statistics. Perhaps the stream was disposed.";
        public CovidStatisticsStreamProcessingException(Exception innerException)
            : base(ExceptionMessage, innerException)
        {

        }

    }
}