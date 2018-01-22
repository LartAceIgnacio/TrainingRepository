using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ZipCodeRequiredException : ApplicationException
    {
        public ZipCodeRequiredException(string message) : base(message)
        {

        }
    }
}
