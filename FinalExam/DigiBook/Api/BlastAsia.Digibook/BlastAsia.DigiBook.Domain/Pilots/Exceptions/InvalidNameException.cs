using System;

namespace BlastAsia.DigiBook.Domain.Pilots.Exceptions
{
    public class InvalidNameException : ApplicationException
    {
        public InvalidNameException(string message) : base(message)
        {
        }
    }
}