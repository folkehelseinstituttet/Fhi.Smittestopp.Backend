using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions
{
    public class CovidStatisticsCsvDataPartiallyMissingException : Exception
    {

        public CovidStatisticsCsvDataPartiallyMissingException(DateTime dateTime) : base($"One of the Csv files is wrong. There is no data for some column, that coresponds to a date: {dateTime}")
        {

        }
    }
}