using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentNameRequiredException: Exception
    {
        public DepartmentNameRequiredException(string message): base(message)
        {

        }
    }
}