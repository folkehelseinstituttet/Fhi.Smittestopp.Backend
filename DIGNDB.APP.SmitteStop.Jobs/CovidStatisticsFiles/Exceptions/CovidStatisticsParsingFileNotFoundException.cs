using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Enums;
using System;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions
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