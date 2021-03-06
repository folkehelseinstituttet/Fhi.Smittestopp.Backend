﻿using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Exceptions
{
    public class CovidStatisticsCsvDataPartiallyMissingException : Exception
    {
        public CovidStatisticsCsvDataPartiallyMissingException(DateTime dateTime) : base(
            $"One of the Csv files is wrong. There is no data for some column, that corresponds to a date: {dateTime}")
        {
        }
    }
}