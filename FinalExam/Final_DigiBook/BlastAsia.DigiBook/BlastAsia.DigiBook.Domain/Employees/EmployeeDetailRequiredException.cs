using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeDetailRequiredException
        :Exception
    {
        public EmployeeDetailRequiredException(string message)
            : base(message)
        {

        }
    }
}