using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class AddressRequiredException : Exception
    {
        public AddressRequiredException(String message) : base(message)
        {

        }
    }
}
