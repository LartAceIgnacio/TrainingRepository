using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentMemberIdRequiredException : ApplicationException
    {
        public DepartmentMemberIdRequiredException(string message)
          : base(message)
        {

        }
    }
}