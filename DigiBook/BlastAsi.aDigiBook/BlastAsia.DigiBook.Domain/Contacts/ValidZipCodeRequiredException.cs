using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ValidZipCodeRequiredException
    : Exception
    {
        public ValidZipCodeRequiredException(string message)
            :base(message)
        {

        }
    }
}