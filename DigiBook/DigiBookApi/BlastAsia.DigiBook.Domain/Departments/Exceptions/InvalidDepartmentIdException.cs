using System;

namespace BlastAsia.DigiBook.Domain.Departments.Exceptions
{
    public class InvalidDepartmentIdException : ApplicationException
    {
        public InvalidDepartmentIdException(string message) : base(message)
        {

        }
    }
}