using System;

namespace BlastAsia.DigiBook.Domain.Contacts.Exception
{
    public class MobilePhoneRequiredException : ApplicationException
    {
        public MobilePhoneRequiredException(string message) : base(message)
        {

        }
    }
}