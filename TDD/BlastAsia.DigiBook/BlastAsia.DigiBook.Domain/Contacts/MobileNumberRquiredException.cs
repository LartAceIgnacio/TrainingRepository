using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class MobileNumberRquiredException
        : Exception
    {
        public MobileNumberRquiredException(string message)
            :base(message)
        {

        }
    }
}