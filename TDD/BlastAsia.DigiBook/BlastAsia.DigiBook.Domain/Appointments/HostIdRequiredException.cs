using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class HostIdRequiredException
        : Exception
    {
        public HostIdRequiredException (string message)
            :base(message)
        {

        }
    }
}