using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class EmailAddressRequiredException
    : Exception
    {
        public EmailAddressRequiredException(string message)
            :base(message)
        {

        }
    }
}