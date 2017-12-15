using System;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class ExtensionRequiredException : Exception
    {
        public ExtensionRequiredException(string message)
            : base(message)
        {

        }
    }
}