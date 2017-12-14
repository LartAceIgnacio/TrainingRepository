using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class InvalidEmailFormatException : ApplicationException
    {
        public InvalidEmailFormatException(string message) : base(message) { }
    }
}
