using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class ContactIdRequiredException : Exception
    {

        public ContactIdRequiredException(string message) : base(message)
        {
        }
    }
}