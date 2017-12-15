using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidGuestIdException
        : Exception
    {
        public InvalidGuestIdException(string message)
            : base(message)
        {

        }
    }
}