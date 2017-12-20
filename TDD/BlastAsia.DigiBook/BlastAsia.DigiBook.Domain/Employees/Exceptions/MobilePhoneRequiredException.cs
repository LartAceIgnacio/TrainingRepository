using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class MobilePhoneRequiredException
        : Exception
    {
        public MobilePhoneRequiredException(string message)
            : base(message)
        {

        }
    }
}