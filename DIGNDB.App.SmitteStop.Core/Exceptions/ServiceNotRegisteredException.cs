using System;

namespace DIGNDB.App.SmitteStop.Core.Exceptions
{
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}