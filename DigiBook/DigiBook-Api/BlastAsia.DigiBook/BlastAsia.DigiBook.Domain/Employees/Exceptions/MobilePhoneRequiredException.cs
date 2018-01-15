using System;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class MobilePhoneRequiredException :Exception
    {
        public MobilePhoneRequiredException(string message)
            : base(message)
        {

        }
    }
}