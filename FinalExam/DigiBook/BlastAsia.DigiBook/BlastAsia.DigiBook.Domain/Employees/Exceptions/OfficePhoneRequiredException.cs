using System;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class OfficePhoneRequiredException: Exception
    {
        public OfficePhoneRequiredException(string message)
            : base(message)
        {

        }
    }
}