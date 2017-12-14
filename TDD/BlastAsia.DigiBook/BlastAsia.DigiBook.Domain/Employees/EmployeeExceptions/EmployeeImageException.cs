using System;

namespace BlastAsia.DigiBook.Domain.Employees.EmployeeExceptions
{
    public class EmployeeImageException : ApplicationException
    {
        public EmployeeImageException(string message) : base(message)
        {

        }
    }
}