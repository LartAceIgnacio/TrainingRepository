using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class MobilePhoneRequiredException : Exception
    {
        public MobilePhoneRequiredException(String message) : base(message)
        {

        }
    }
}
