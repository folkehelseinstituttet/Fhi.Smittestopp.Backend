﻿using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Exceptions
{
    public class CovidStatisticsCsvDataNotUniqueException : Exception
    {

        public CovidStatisticsCsvDataNotUniqueException(DateTime dateTime, Exception innerException) :
            base($"One of the Csv files is wrong. There is multiple entries for some column, that coresponds to a date: {dateTime}", innerException)
        {

        }
    }
}