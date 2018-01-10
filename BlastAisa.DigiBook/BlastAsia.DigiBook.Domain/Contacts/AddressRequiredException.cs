using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class AddressRequiredException : ApplicationException
    {
        public AddressRequiredException(string message) : base (message)
        {

        }
    }
}
