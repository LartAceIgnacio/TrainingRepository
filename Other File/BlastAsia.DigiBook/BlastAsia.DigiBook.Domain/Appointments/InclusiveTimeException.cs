using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InclusiveTimeException : Exception
    {
        public InclusiveTimeException(string message) : base(message)
        {
        }
    }
}