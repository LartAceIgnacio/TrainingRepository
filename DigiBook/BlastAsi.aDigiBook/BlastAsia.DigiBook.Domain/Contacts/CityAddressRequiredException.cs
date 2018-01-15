using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CityAddressRequiredException
        : Exception
    {
        public CityAddressRequiredException(string message)
            :base(message)
        {

        }
    }
}