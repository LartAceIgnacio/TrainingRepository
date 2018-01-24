using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Employees.EmployeeExceptions
{
    [Serializable]
    public class ExtensionNumberException : ApplicationException
    {
        public ExtensionNumberException(string message) : base(message)
        {
        }
        
    }
}