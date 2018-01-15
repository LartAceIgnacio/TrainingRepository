using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class InvalidEmailAddressException: Exception
    {
        public InvalidEmailAddressException(string message)
            :base(message)
        {

        }
    }
}