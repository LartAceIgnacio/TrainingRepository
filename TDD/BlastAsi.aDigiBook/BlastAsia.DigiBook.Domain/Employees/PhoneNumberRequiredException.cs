using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class PhoneNumberRequiredException
        : Exception
    {
        public PhoneNumberRequiredException(string message)
            : base(message)
        {
        }
    }
}