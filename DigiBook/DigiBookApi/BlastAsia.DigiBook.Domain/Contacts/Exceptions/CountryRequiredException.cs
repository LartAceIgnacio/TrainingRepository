using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class CountryRequiredException : ApplicationException
    {
        public CountryRequiredException(string message) : base(message)
        {

        }
    }
}
