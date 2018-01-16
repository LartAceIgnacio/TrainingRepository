using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class StreetAddressRequiredException : ApplicationException
    {
        public StreetAddressRequiredException(string message) : base(message)
        {

        }
    }
}
