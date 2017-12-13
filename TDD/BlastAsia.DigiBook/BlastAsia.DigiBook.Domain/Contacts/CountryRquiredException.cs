using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CountryRquiredException
        :Exception
    {
        public CountryRquiredException(string messege)
            :base(messege)
        {

        }
    }
}