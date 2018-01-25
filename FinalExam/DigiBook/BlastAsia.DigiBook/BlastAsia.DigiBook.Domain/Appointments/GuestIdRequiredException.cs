using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class GuestIdRequiredException
        : Exception
    {
        public GuestIdRequiredException(string message)
            :base(message)
        {

        }
    }
}