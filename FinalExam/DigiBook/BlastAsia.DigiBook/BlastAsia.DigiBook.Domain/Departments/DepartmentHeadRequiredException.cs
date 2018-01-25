using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentHeadRequiredException: Exception
    {
        public DepartmentHeadRequiredException(string message): base(message)
        {

        }
    }
}