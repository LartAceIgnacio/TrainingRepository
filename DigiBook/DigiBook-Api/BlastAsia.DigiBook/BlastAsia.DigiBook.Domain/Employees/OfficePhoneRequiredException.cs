using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class OfficePhoneRequiredException
        :Exception
    {
        public OfficePhoneRequiredException(string message)
            :base(message)
        {

        }
    }
}