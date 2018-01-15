using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CountryRequiredException
        :Exception
    {
        public CountryRequiredException(string message)
            :base(message)
        {
                
        }
    }
}