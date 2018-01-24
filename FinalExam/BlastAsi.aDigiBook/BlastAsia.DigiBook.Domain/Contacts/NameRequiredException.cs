using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class NameRequiredException
        :Exception
    {
        public NameRequiredException(string message)
            :base(message)
        {

        }
    }
}