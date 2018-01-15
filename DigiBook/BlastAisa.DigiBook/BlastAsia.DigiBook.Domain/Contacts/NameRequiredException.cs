using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NameRequiredException : ApplicationException
    {
        public NameRequiredException(string message) : base(message)
        {

        }
    }
}
