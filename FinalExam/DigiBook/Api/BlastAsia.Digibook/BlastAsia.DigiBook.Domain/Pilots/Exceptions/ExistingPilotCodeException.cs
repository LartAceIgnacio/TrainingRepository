using System;

namespace BlastAsia.DigiBook.Domain.Pilots.Exceptions
{
    public class ExistingPilotCodeException : ApplicationException
    {
        public ExistingPilotCodeException(string message) : base(message)
        {
        }
    }
}