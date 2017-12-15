using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CountryRequiredException : Exception
    {
        public CountryRequiredException(String message) : base(message)
        {

        }
    }
}
