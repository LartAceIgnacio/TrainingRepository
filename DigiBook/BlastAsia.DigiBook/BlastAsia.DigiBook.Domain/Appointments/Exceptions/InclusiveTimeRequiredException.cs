using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InclusiveTimeRequiredException
        : Exception
    {
        public InclusiveTimeRequiredException(string message)
            : base(message)
        {

        }
    }
}