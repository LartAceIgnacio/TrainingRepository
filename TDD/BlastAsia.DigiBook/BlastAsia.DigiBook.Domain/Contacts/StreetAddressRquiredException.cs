using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class StreetAddressRquiredException
        :Exception
    {
        public StreetAddressRquiredException(string message)
            :base(message)
        {

        }
    }
}