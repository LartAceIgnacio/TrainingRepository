using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class PhoneRequiredException: Exception
    {
        public PhoneRequiredException(string message)
            :base(message)
        {

        }
    }
}