using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NagativeZipCodeException: Exception
    {
        public NagativeZipCodeException(string message)
            : base(message)
        {

        }
    }
}