using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions
{
    public class CovidStatisticsCsvDataNotUniqueException : Exception
    {

        public CovidStatisticsCsvDataNotUniqueException(DateTime dateTime, Exception innerException) :
            base($"One of the Csv files is wrong. There is multiple entries for some column, that corresponds to a date: {dateTime}", innerException)
        {
        }
    }
}