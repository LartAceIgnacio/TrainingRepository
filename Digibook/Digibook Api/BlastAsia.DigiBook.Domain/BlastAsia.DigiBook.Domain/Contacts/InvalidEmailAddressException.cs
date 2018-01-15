using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class InvalidEmailAddressException: Exception
    {
        public InvalidEmailAddressException(string message)
            : base(message)
        {

        }
    }
}