using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Enums;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Exceptions
{
    [Serializable]
    public class CovidStatisticsParsingFileNotFoundException : Exception
    {
        public CovidStatisticsParsingFileNotFoundException(CovidStatisticsFileName name)
            : base(String.Format("Could not find file with name: {0}", name))
        {

        }

    }
}