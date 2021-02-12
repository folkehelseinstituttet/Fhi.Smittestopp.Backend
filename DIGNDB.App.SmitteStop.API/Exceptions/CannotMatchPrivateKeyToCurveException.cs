using System;
using System.Runtime.Serialization;

namespace DIGNDB.App.SmitteStop.API.Exceptions
{
    [Serializable]
    public class CannotMatchPrivateKeyToCurveException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CannotMatchPrivateKeyToCurveException()
        {
        }

        public CannotMatchPrivateKeyToCurveException(string message) : base(message)
        {
        }

        public CannotMatchPrivateKeyToCurveException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CannotMatchPrivateKeyToCurveException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}