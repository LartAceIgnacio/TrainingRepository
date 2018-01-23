using System;

namespace BlastAsia.DigiBook.Domain.Departments.Exceptions
{
    public class DepartmentDescriptionException : Exception
    {
        public DepartmentDescriptionException(string message)
            : base(message)
        {

        }
    }
}