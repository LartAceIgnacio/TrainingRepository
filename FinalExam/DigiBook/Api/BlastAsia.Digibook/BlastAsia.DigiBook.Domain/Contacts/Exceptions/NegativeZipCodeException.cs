using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NegativeZipCodeException : ApplicationException
    {
        public NegativeZipCodeException(string message) : base(message)
        {

        }
    }
}
