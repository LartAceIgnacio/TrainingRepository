using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class CityAddressRequiredException : ApplicationException
    {
        public CityAddressRequiredException(string message)
            : base(message) { }
        
    }
}
