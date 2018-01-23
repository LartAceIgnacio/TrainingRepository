using System;

namespace BlastAsia.DigiBook.Domain.Pilots.Exceptions
{
    public class InvalidPilotCodeException : ApplicationException
    {
        public InvalidPilotCodeException(string message) : base(message)
        {
        }
    }
}