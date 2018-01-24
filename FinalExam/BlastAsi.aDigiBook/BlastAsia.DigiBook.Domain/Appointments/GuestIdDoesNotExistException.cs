using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class GuestIdDoesNotExistException
    : Exception
    {
        public GuestIdDoesNotExistException(string message)
            :base(message)
        {

        }
    }
}