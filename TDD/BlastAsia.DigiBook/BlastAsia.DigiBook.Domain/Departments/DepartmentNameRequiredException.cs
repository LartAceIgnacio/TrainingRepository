using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentNameRequiredException
        : Exception
    {
        public DepartmentNameRequiredException(string message)
            : base(message)
        {

        }
    }
}
