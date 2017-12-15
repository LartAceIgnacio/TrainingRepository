using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class ValidEmailAddressRequiredException
        :Exception
    {
        public ValidEmailAddressRequiredException(string message)
            :base(message)
        {

        }
    }
}