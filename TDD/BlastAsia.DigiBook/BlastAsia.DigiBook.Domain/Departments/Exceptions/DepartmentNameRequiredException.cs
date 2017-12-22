using System;

namespace BlastAsia.DigiBook.Domain.Departments.Exceptions
{
    public class DepartmentNameRequiredException : Exception
    {
        public DepartmentNameRequiredException(string message)
            : base(message)
        {

        }
    }
}