using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CityAddressRquiredException
        :Exception
    {
        public CityAddressRquiredException(string message)
            :base(message)
        {

        }
    }
}