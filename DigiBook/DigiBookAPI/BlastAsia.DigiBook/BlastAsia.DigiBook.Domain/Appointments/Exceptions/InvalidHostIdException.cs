using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidHostIdException
        : Exception
    {
        public InvalidHostIdException(string message)
            : base(message)
        {

        }
    }
}