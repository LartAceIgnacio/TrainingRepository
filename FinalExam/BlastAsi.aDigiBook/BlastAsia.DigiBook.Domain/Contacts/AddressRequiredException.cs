using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class AddressRequiredException
        : Exception
    {
        public AddressRequiredException(string message)
            :base(message)
        {

        }
    }
}