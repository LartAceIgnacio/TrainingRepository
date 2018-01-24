using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class ContactNumberRequiredException : ApplicationException
    {
        public ContactNumberRequiredException(string message) 
            : base(message) {}
    }
}
