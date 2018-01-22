using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class InvalidGuestIdException : ApplicationException
    {
        public InvalidGuestIdException(string message) : base(message)
        {

        }
    }
}
