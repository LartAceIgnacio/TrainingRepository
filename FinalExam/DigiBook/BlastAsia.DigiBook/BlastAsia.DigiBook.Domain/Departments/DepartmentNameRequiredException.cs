using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentNameRequiredException : ApplicationException
    {
        public DepartmentNameRequiredException(string message)
           : base(message)
        {

        }
    }
}