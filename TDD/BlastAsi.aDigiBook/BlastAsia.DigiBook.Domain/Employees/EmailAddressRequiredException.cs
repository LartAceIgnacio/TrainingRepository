using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmailAddressRequiredException
        :Exception
    {
        public EmailAddressRequiredException(string message)
            :base (message)
        {

        }
    }
}