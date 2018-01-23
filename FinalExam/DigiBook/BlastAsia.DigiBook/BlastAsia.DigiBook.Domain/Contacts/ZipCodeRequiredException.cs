using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ZipCodeRequiredException : Exception
    {
        public ZipCodeRequiredException(string message)
            : base(message)
        {

        }
    }
}