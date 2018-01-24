using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts.ContactExceptions
{
    public class InvalidZipCodeException : ApplicationException
    {

        public InvalidZipCodeException(string message) : base(message){ }
    }
}
