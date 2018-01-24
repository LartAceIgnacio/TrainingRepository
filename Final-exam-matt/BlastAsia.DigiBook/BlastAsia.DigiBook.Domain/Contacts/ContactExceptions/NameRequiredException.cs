using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class NameRequiredException : ApplicationException
    {
        public NameRequiredException(string message) : base(message) {}
    }
}
