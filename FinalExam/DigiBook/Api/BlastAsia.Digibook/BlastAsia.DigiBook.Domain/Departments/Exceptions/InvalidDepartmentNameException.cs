using System;

namespace BlastAsia.DigiBook.Domain.Departments.Exceptions
{
    public class InvalidDepartmentNameException : ApplicationException
    {
        public InvalidDepartmentNameException(string message) : base(message)
        {

        }
    }
}