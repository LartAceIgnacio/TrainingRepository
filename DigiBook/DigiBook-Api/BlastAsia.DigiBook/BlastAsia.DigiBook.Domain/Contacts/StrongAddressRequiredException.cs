using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class StrongAddressRequiredException : Exception
    {
        public StrongAddressRequiredException(string message)
            : base(message)
        {

        }
    }
}