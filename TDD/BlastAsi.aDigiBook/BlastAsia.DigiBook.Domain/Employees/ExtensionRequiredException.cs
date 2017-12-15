using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class ExtensionRequiredException
        : Exception
    {
        public ExtensionRequiredException(string message)
            :base(message)
        {

        }
    }
}