using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class PositiveZipCodeRequiredException
        : Exception
    {
        public PositiveZipCodeRequiredException(string message)
            :base (message)
        {

        }
    }
}