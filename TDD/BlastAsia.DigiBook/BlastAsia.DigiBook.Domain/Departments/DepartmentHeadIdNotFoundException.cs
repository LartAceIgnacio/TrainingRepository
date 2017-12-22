using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentHeadIdNotFoundException: Exception
    {
        public DepartmentHeadIdNotFoundException(string message)
            :base(message)
        {

        }
    }
}