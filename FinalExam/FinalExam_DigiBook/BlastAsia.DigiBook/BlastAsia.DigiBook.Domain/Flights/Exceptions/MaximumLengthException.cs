using System;

namespace BlastAsia.DigiBook.Domain.Exceptions
{
    public class MaximumLengthException : Exception
    {
        public MaximumLengthException(string message)
            : base(message)
        {

        }
    }
}