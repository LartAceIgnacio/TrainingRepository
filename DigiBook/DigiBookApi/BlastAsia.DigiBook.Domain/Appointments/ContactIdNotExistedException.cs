using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class ContactIdNotExistedException : Exception
    {
        public ContactIdNotExistedException(string message) 
            : base(message)
        {

        }
    }
}