using System;

namespace DIGNDB.APP.SmitteStop.API.Exceptions
{
    public class GitHubControllerServerErrorException : Exception
    {
        public GitHubControllerServerErrorException(string message) : base(message)
        {
        }

        public GitHubControllerServerErrorException(string message, Exception e) : base(message, e)
        {
        }
    }
}