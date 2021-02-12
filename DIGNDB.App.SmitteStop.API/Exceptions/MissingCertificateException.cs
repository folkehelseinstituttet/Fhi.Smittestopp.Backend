using System;
using System.Runtime.Serialization;

namespace DIGNDB.App.SmitteStop.API.Exceptions
{
    [Serializable]
    public class MissingCertificateException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MissingCertificateException()
        {
        }

        public MissingCertificateException(string message) : base(message)
        {
        }

        public MissingCertificateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingCertificateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}