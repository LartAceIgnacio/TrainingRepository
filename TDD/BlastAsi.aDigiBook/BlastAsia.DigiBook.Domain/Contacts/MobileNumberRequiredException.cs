using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class MobileNumberRequiredException
        : Exception
    {
        public MobileNumberRequiredException(string message)
            :base(message)
        {

        }
    }
}