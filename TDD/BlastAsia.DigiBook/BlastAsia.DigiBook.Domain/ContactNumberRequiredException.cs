using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class ContactNumberRequiredException : ApplicationException
    {
        public ContactNumberRequiredException(string message) 
            : base(message) {}
    }
}
