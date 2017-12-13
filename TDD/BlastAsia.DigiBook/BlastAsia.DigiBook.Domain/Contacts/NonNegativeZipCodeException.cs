using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NonNegativeZipCodeException
        :Exception
    {
        public NonNegativeZipCodeException(string message)
            : base(message)
        {

        }
    }
}