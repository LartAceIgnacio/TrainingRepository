using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class ExtensionRequiredException : Exception
    {
        public ExtensionRequiredException(String message) : base(message)
        {

        }
    }
}
