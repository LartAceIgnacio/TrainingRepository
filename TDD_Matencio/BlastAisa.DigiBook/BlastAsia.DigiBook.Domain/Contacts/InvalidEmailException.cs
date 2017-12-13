using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class InvalidEmailException : ApplicationException
    {
        public InvalidEmailException(string message) : base (message)
        {

        }
    }
}
