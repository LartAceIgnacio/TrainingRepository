using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class AddressException : Exception
    {
        public AddressException(String message) : base(message)
        {

        }
    }
}
