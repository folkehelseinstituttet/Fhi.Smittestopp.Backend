using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Exceptions
{
    public class CovidStatisticsCsvContentMultipleContentsOfTheSameTypeException : Exception
    {
        private const string ExceptionMessage =
            "CovidStatisticsContent cannot contain multiple definitions for files of the same type";

        public CovidStatisticsCsvContentMultipleContentsOfTheSameTypeException() : base(ExceptionMessage)
        {

        }
    }
}