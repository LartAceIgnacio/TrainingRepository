using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Pilots.Pilots.Exceptions
{
    [Serializable]
    public class RequiredException : Exception
    {
        public RequiredException()
        {
        }

        public RequiredException(string message) : base(message)
        {
        }

        public RequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}