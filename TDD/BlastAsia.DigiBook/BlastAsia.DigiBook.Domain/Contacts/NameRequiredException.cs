using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NameRequiredException : Exception
    {
        public NameRequiredException(String message) : base(message)
        {

        }
    }
}
