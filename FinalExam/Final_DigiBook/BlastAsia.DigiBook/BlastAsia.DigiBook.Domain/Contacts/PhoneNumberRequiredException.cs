using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class PhoneNumberRequiredException
        :Exception
    {
        public PhoneNumberRequiredException(string message)
            :base(message)
        {

        }
    }
}