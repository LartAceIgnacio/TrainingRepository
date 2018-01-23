using System;

namespace BlastAsia.DigiBook.Domain.Pilots.Exceptions
{
    public class InvalidDateException : ApplicationException
    {
        public InvalidDateException(string message) : base(message)
        {
        }
    }
}