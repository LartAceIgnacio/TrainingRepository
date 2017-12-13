using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException(String message) : base(message)
        {

        }
    }
}
