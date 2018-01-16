using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class MobilePhoneRequiredException
        : Exception
    {
        public MobilePhoneRequiredException(string message)
            : base(message)
        {

        } 
    }
}