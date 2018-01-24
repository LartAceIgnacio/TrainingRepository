using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class AddressRequiredException : ApplicationException
    {
        public AddressRequiredException(string message) 
            : base(message) {}
    }
}
