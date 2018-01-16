using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class InvalidHostIdException : ApplicationException
    {
        public InvalidHostIdException(string message) : base(message)
        {

        }
    }
}
