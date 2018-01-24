using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class ContactNumberMinimumLength : ApplicationException
    {
        public ContactNumberMinimumLength(string message) : base(message)
        {

        }
    }
}
