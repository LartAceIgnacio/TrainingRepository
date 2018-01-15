using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class MobilePhoneRequiredException : ApplicationException
    {
        public MobilePhoneRequiredException(string message) : base (message)
        {

        }
    }
}
